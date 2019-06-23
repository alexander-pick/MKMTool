/*
	This file is part of MKMTool

    MKMTool is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MKMTool is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

    Diese Datei ist Teil von MKMTool.

    MKMTool ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder späteren
    veröffentlichten Version, weiterverbreiten und/oder modifizieren.

    Fubar wird in der Hoffnung, dass es nützlich sein wird, aber
    OHNE JEDE GEWÄHRLEISTUNG, bereitgestellt; sogar ohne die implizite
    Gewährleistung der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK.
    Siehe die GNU General Public License für weitere Details.

    Sie sollten eine Kopie der GNU General Public License zusammen mit diesem
    Programm erhalten haben. Wenn nicht, siehe <http://www.gnu.org/licenses/>.
*/
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace MKMTool
{
    public partial class CheckDisplayPrices : Form
    {
        private readonly DataTable eS = new DataTable();
        private readonly MainView frm1;

        public CheckDisplayPrices(MainView frm)
        {
            frm1 = frm;

            InitializeComponent();

            try
            {
                var doc = MKMInteract.RequestHelper.getExpansions("1"); // Only MTG at present

                var node = doc.GetElementsByTagName("expansion");

                eS.Columns.Add("idExpansion", typeof (string));
                eS.Columns.Add("abbreviation", typeof (string));
                eS.Columns.Add("enName", typeof (string));

                foreach (XmlNode nExpansion in node)
                {
                    eS.Rows.Add(nExpansion["idExpansion"].InnerText, nExpansion["abbreviation"].InnerText,
                        nExpansion["enName"].InnerText);
                }

                foreach (XmlNode nExpansion in node)
                {
                    var item = new MKMHelpers.ComboboxItem();

                    item.Text = nExpansion["enName"].InnerText;
                    item.Value = nExpansion["idExpansion"].InnerText;

                    editionBox.Items.Add(item);
                }

                editionBox.Sorted = true;
                editionBox.SelectedIndex = 135; // currently Kaladesh
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("initializing expansions for display price check", eError.Message, true);
            }
        }

        private void checkDipslayPrice_Click(object sender, EventArgs e)
        {
            try
            {
                //used to determine index of best start edition
                //MessageBox.Show((editionBox.SelectedIndex.ToString()));

                var doc =
                    MKMInteract.RequestHelper.getExpansionsSingles((editionBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString());
                //would be easier if mkm would deliver detailed info with this call but ...

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "====== Extension Stats ======\n");

                //Multiplier 
                float fCardsInSet = doc.SelectNodes("response/single").Count;
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Cards in set: " + fCardsInSet + "\n");

                var fMyticFactor = (float) Convert.ToDouble(myticPerRareText.Text); // 1 out of 8 packs has a mytic
                var fPackUncommon = (float) Convert.ToDouble(uncommonPerPackText.Text);
                var fPackRareMytic = (float) Convert.ToDouble(raremyticPerPackText.Text);

                var fRareCardsNotinPacks = (float) Convert.ToDouble(rareNotInBoosterText.Text); //rare and mytic
                var fMyticCardsNotinPacks = (float) Convert.ToDouble(mythicNotInBoosterText.Text); //rare and mytic
                var fUncommonCardsNotinPacks = (float) Convert.ToDouble(uncommonNotInBoosterText.Text); //rare and mytic

                var fBoxContent = (float) Convert.ToDouble(boosterPerBoxText.Text); //36 Packs

                var xRares = doc.SelectNodes("response/single/rarity[. = \"Rare\"]");
                var xMythic = doc.SelectNodes("response/single/rarity[. = \"Mythic\"]");
                var xUncommon = doc.SelectNodes("response/single/rarity[. = \"Uncommon\"]");

                var iCountRares = xRares.Count - fRareCardsNotinPacks; //53F;
                var iCountMytics = xMythic.Count - fMyticCardsNotinPacks; //15F;
                var iCountUncommons = xUncommon.Count - fUncommonCardsNotinPacks; //80F;

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Rares in set: " + iCountRares + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Mytic in set: " + iCountMytics + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Uncommon in set: " + iCountUncommons + "\n");

                //factors per booster
                var fFactorUncommon = fPackUncommon/iCountUncommons; //0,0375
                var fFactorMyticRareCombined = fPackRareMytic/(iCountRares + iCountMytics); // 0,014
                var fFactorMytic = fFactorMyticRareCombined/fMyticFactor; //chance is 1:8 fpr Mytic
                var fFactorRare = fFactorMyticRareCombined/fMyticFactor*(fMyticFactor - 1);

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "====== Calculated Booster Factors ======\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Uncommon: " + fFactorUncommon + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "MR Combo: " + fFactorMyticRareCombined + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Rare:" + fFactorRare + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Mytic:" + fFactorMytic + "\n");

                var fFactorUncommonBox = fFactorUncommon*fBoxContent;
                var fFactorMyticRareCombinedBox = fFactorMyticRareCombined*fBoxContent;
                var fFactorMyticBox = fFactorMytic*fBoxContent;
                var fFactorRareBox = fFactorRare*fBoxContent;

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "====== Calculated Box Factors ======\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Uncommon: " + fFactorUncommonBox + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "MR Combo: " + fFactorMyticRareCombinedBox + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Rare:" + fFactorRareBox + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Mytic:" + fFactorMyticBox + "\n");

                xRares = doc.SelectNodes("response/single");

                float fBoxValue = 0;

                foreach (XmlNode xn in xRares)
                {
                    if (xn["rarity"].InnerText == "Rare")
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Checking (R): " + xn["enName"].InnerText + "\n");

                        var doc2 =
                            MKMInteract.RequestHelper.makeRequest(
                                "https://api.cardmarket.com/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        var fCardPrice =
                            (float)
                                Convert.ToDouble(
                                    doc2.SelectSingleNode("response/product/priceGuide/SELL")
                                        .InnerText.Replace(".", ","));

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Current SELL Price: " + fCardPrice + "\n");

                        fBoxValue += fCardPrice*fFactorMyticRareCombinedBox;
                            //changed cause it's actually a rare + mytic not rare or mytiv I think?  was fFactorRareBox;
                    }

                    if (xn["rarity"].InnerText == "Mythic")
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Checking (M): " + xn["enName"].InnerText + "\n");

                        var doc2 =
                            MKMInteract.RequestHelper.makeRequest(
                                "https://api.cardmarket.com/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        var fCardPrice =
                            (float)
                                Convert.ToDouble(
                                    doc2.SelectSingleNode("response/product/priceGuide/SELL")
                                        .InnerText.Replace(".", ","));

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Current SELL Price: " + fCardPrice + "\n");

                        fBoxValue += fCardPrice*fFactorMyticBox;
                    }

                    if (xn["rarity"].InnerText == "Uncommon")
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Checking (U): " + xn["enName"].InnerText + "\n");

                        var doc2 =
                            MKMInteract.RequestHelper.makeRequest(
                                "https://api.cardmarket.com/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        var fCardPrice =
                            (float)
                                Convert.ToDouble(
                                    doc2.SelectSingleNode("response/product/priceGuide/SELL")
                                        .InnerText.Replace(".", ","));

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Current SELL Price: " + fCardPrice + "\n");

                        fBoxValue += fCardPrice*fFactorUncommonBox;
                    }
                }
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Calculated Result *: " + fBoxValue + "\n");

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "* Estimated average booster box singles value at MKM SELL Pricing (EUR)\n");
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("checking display prices", eError.Message, true);
            }
        }
    }
}