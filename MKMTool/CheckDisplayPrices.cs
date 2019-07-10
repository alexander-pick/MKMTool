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
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MKMTool
{
    public partial class CheckDisplayPrices : Form
    { 

        public CheckDisplayPrices()
        {
            InitializeComponent();

            MKMDbManager.Instance.PopulateExpansionsComboBox(editionBox);

            editionBox.Sorted = true;
            editionBox.SelectedIndex = 135; // currently Gatecrash
        }

        private async void checkDisplayPrice_Click(object sender, EventArgs e)
        {
            checkDisplayPrice.Enabled = false;

            // the window controls can't be accessed from a different thread -> have to parse them here and send as arguments
            var fMythicFactor = (float)Convert.ToDouble(mythicPerRareText.Text, CultureInfo.InvariantCulture); // 1 out of 8 packs has a Mythic
            var fPackUncommon = (float)Convert.ToDouble(uncommonPerPackText.Text, CultureInfo.InvariantCulture);
            var fPackRareMythic = (float)Convert.ToDouble(raremythicPerPackText.Text, CultureInfo.InvariantCulture);

            var fRareCardsNotinPacks = (float)Convert.ToDouble(rareNotInBoosterText.Text, CultureInfo.InvariantCulture); //rare and Mythic
            var fMythicCardsNotinPacks = (float)Convert.ToDouble(mythicNotInBoosterText.Text, CultureInfo.InvariantCulture); //rare and Mythic
            var fUncommonCardsNotinPacks = (float)Convert.ToDouble(uncommonNotInBoosterText.Text, CultureInfo.InvariantCulture); //rare and Mythic

            var fBoxContent = (float)Convert.ToDouble(boosterPerBoxText.Text, CultureInfo.InvariantCulture); //36 Packs
            string editionID = (editionBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();
            
            // run the check in a separate thread so that the main GUI is correctly updated
            await Task.Run(() => checkDisplayPrice_run(fMythicFactor, fPackUncommon, fPackRareMythic, fRareCardsNotinPacks, fMythicCardsNotinPacks,
                fUncommonCardsNotinPacks, fBoxContent, editionID));
            checkDisplayPrice.Enabled = true;
        }

        /// <summary>
        /// Checks the expected ROI of a given display. Expected to be run in a separate thread
        /// </summary>
        private void checkDisplayPrice_run(float fMythicFactor, float fPackUncommon, float fPackRareMythic, float fRareCardsNotinPacks,
            float fMythicCardsNotinPacks, float fUncommonCardsNotinPacks, float fBoxContent, string editionID)
        {
            try
            {
                //used to determine index of best start edition
                //MessageBox.Show((editionBox.SelectedIndex.ToString()));

                var doc = MKMInteract.RequestHelper.getExpansionsSingles(editionID);
                //would be easier if mkm would deliver detailed info with this call but ...

                MainView.Instance.LogMainWindow("====== Expansion Stats ======");

                //Multiplier 
                float fCardsInSet = doc.SelectNodes("response/single").Count;
                MainView.Instance.LogMainWindow("Cards in set: " + fCardsInSet);


                var xRares = doc.SelectNodes("response/single/rarity[. = \"Rare\"]");
                var xMythic = doc.SelectNodes("response/single/rarity[. = \"Mythic\"]");
                var xUncommon = doc.SelectNodes("response/single/rarity[. = \"Uncommon\"]");

                var iCountRares = xRares.Count - fRareCardsNotinPacks; //53F;
                var iCountMythics = xMythic.Count - fMythicCardsNotinPacks; //15F;
                var iCountUncommons = xUncommon.Count - fUncommonCardsNotinPacks; //80F;

                MainView.Instance.LogMainWindow("Rares in set: " + iCountRares);
                MainView.Instance.LogMainWindow("Mythic in set: " + iCountMythics);
                MainView.Instance.LogMainWindow("Uncommon in set: " + iCountUncommons);

                //factors per booster
                var fFactorUncommon = fPackUncommon/iCountUncommons; //0,0375
                var fFactorMythicRareCombined = fPackRareMythic/(iCountRares + iCountMythics); // 0,014
                var fFactorMythic = fFactorMythicRareCombined/fMythicFactor; //chance is 1:8 fpr Mythic
                var fFactorRare = fFactorMythicRareCombined/fMythicFactor*(fMythicFactor - 1);

                MainView.Instance.LogMainWindow("====== Calculated Booster Factors ======");
                MainView.Instance.LogMainWindow("Uncommon: " + fFactorUncommon);
                MainView.Instance.LogMainWindow("MR Combo: " + fFactorMythicRareCombined);
                MainView.Instance.LogMainWindow( "Rare:" + fFactorRare);
                MainView.Instance.LogMainWindow( "Mythic:" + fFactorMythic);

                var fFactorUncommonBox = fFactorUncommon*fBoxContent;
                var fFactorMythicRareCombinedBox = fFactorMythicRareCombined*fBoxContent;
                var fFactorMythicBox = fFactorMythic*fBoxContent;
                var fFactorRareBox = fFactorRare*fBoxContent;

                MainView.Instance.LogMainWindow("====== Calculated Box Factors ======");
                MainView.Instance.LogMainWindow("Uncommon: " + fFactorUncommonBox);
                MainView.Instance.LogMainWindow("MR Combo: " + fFactorMythicRareCombinedBox);
                MainView.Instance.LogMainWindow("Rare:" + fFactorRareBox);
                MainView.Instance.LogMainWindow("Mythic:" + fFactorMythicBox);

                xRares = doc.SelectNodes("response/single");

                float fBoxValue = 0;

                foreach (XmlNode xn in xRares)
                {
                    if (xn["rarity"].InnerText == "Rare")
                    {
                        MainView.Instance.LogMainWindow("Checking (R): " + xn["enName"].InnerText);

                        var doc2 =
                            MKMInteract.RequestHelper.makeRequest(
                                "https://api.cardmarket.com/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        if (doc2.HasChildNodes)
                        {
                            var fCardPrice =
                                (float)Convert.ToDouble(
                                        doc2.SelectSingleNode("response/product/priceGuide/SELL").InnerText, CultureInfo.InvariantCulture);

                            MainView.Instance.LogMainWindow("Current SELL Price: " + fCardPrice);

                            fBoxValue += fCardPrice * fFactorMythicRareCombinedBox;
                            //changed cause it's actually a rare + Mythic not rare or mythic I think?  was fFactorRareBox;
                        }
                    }

                    if (xn["rarity"].InnerText == "Mythic")
                    {
                        MainView.Instance.LogMainWindow("Checking (M): " + xn["enName"].InnerText);

                        var doc2 =
                            MKMInteract.RequestHelper.makeRequest(
                                "https://api.cardmarket.com/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        if (doc2.HasChildNodes)
                        {
                            var fCardPrice =
                                (float)Convert.ToDouble(
                                        doc2.SelectSingleNode("response/product/priceGuide/SELL").InnerText, CultureInfo.InvariantCulture);
                            MainView.Instance.LogMainWindow("Current SELL Price: " + fCardPrice);

                            fBoxValue += fCardPrice * fFactorMythicBox;
                        }
                    }

                    if (xn["rarity"].InnerText == "Uncommon")
                    {
                        MainView.Instance.LogMainWindow("Checking (U): " + xn["enName"].InnerText);

                        var doc2 =
                            MKMInteract.RequestHelper.makeRequest(
                                "https://api.cardmarket.com/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        if (doc2.HasChildNodes)
                        {
                            var fCardPrice =
                                (float)Convert.ToDouble(
                                        doc2.SelectSingleNode("response/product/priceGuide/SELL").InnerText, CultureInfo.InvariantCulture);

                            MainView.Instance.LogMainWindow("Current SELL Price: " + fCardPrice);

                            fBoxValue += fCardPrice * fFactorUncommonBox;
                        }
                    }
                }
                MainView.Instance.LogMainWindow("Calculated Result *: " + fBoxValue);

                MainView.Instance.LogMainWindow(
                    "* Estimated average booster box singles value at MKM SELL Pricing (EUR)\n");
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("checking display prices", eError.Message, true);
            }
        }
    }
}