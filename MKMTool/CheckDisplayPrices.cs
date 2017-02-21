using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MKMTool
{
    public partial class CheckDisplayPrices : Form
    {
        DataTable eS = new DataTable();
        private MainView frm1;

        public CheckDisplayPrices(MainView frm)
        {
            frm1 = frm;

            InitializeComponent();

            try
            {
                MKMBot bot = new MKMBot();

                XmlDocument doc = bot.getExpansions("1"); // Only MTG at present

                XmlNodeList node = doc.GetElementsByTagName("expansion");

                eS.Columns.Add("idExpansion", typeof(string));
                eS.Columns.Add("abbreviation", typeof(string));
                eS.Columns.Add("enName", typeof(string));

                foreach (XmlNode nExpansion in node)
                {
                    eS.Rows.Add(nExpansion["idExpansion"].InnerText, nExpansion["abbreviation"].InnerText,
                        nExpansion["enName"].InnerText);
                }

                foreach (XmlNode nExpansion in node)
                {
                    MKMHelpers.ComboboxItem item = new MKMHelpers.ComboboxItem();

                    item.Text = nExpansion["enName"].InnerText;
                    item.Value = nExpansion["idExpansion"].InnerText;

                    editionBox.Items.Add(item);
                }

                editionBox.Sorted = true;
                editionBox.SelectedIndex = 135; // currently Kaladesh

            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.Message);

            }


        }

        private void checkDipslayPrice_Click(object sender, EventArgs e)
        {
            try
            {

                MKMBot bot = new MKMBot();

                //used to determain index of best start edition
                //MessageBox.Show((editionBox.SelectedIndex.ToString()));

                XmlDocument doc =
                    bot.getExpansionsSingles((editionBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString());
                //would be easier if mkm would deliver detailed info with this call but ...

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "====== Extension Stats ======\n");

                //Multiplier 
                float fCardsInSet = doc.SelectNodes("response/single").Count;
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Cards in set: " + fCardsInSet + "\n");

                float fMyticFactor = (float)Convert.ToDouble(myticPerRareText.Text); // 1 out of 8 packs has a mytic
                float fPackUncommon = (float)Convert.ToDouble(uncommonPerPackText.Text);
                float fPackRareMytic = (float)Convert.ToDouble(raremyticPerPackText.Text);

                float fRareCardsNotinPacks = (float)Convert.ToDouble(rareNotInBoosterText.Text); //rare and mytic
                float fMyticCardsNotinPacks = (float)Convert.ToDouble(mythicNotInBoosterText.Text); //rare and mytic
                float fUncommonCardsNotinPacks = (float)Convert.ToDouble(uncommonNotInBoosterText.Text); //rare and mytic

                float fBoxContent = (float)Convert.ToDouble(boosterPerBoxText.Text); //36 Packs

                XmlNodeList xRares = doc.SelectNodes("response/single/rarity[. = \"Rare\"]");
                XmlNodeList xMythic = doc.SelectNodes("response/single/rarity[. = \"Mythic\"]");
                XmlNodeList xUncommon = doc.SelectNodes("response/single/rarity[. = \"Uncommon\"]");

                float iCountRares = xRares.Count - fRareCardsNotinPacks; //53F;
                float iCountMytics = xMythic.Count - fMyticCardsNotinPacks; //15F;
                float iCountUncommons = xUncommon.Count - fUncommonCardsNotinPacks; //80F;

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Rares in set: " + iCountRares + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Mytic in set: " + iCountMytics + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Uncommon in set: " + iCountUncommons + "\n");

                //factors per booster
                float fFactorUncommon = fPackUncommon / iCountUncommons; //0,0375
                float fFactorMyticRareCombined = fPackRareMytic / (iCountRares + iCountMytics); // 0,014
                float fFactorMytic = fFactorMyticRareCombined / fMyticFactor; //chance is 1:8 fpr Mytic
                float fFactorRare = (fFactorMyticRareCombined / fMyticFactor) * (fMyticFactor - 1);

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "====== Calculated Booster Factors ======\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Uncommon: " + fFactorUncommon + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "MR Combo: " + fFactorMyticRareCombined + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Rare:" + fFactorRare + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Mytic:" + fFactorMytic + "\n");

                float fFactorUncommonBox = fFactorUncommon * fBoxContent;
                float fFactorMyticRareCombinedBox = fFactorMyticRareCombined * fBoxContent;
                float fFactorMyticBox = fFactorMytic * fBoxContent;
                float fFactorRareBox = fFactorRare * fBoxContent;

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "====== Calculated Box Factors ======\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Uncommon: " + fFactorUncommonBox + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "MR Combo: " + fFactorMyticRareCombinedBox + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Rare:" + fFactorRareBox + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Mytic:" + fFactorMyticBox + "\n");

                xRares = doc.SelectNodes("response/single");

                float fBoxValue = 0;

                foreach (XmlNode xn in xRares)
                {
                    if (xn["rarity"].InnerText == "Rare")
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Checking (R): " + xn["enName"].InnerText + "\n");

                        XmlDocument doc2 = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        float fCardPrice = (float)Convert.ToDouble(doc2.SelectSingleNode("response/product/priceGuide/SELL").InnerText.Replace(".", ","));

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Current SELL Price: " + fCardPrice + "\n");

                        fBoxValue += fCardPrice * fFactorRareBox;

                    }

                    if (xn["rarity"].InnerText == "Mythic")
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Checking (M): " + xn["enName"].InnerText + "\n");

                        XmlDocument doc2 = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        float fCardPrice = (float)Convert.ToDouble(doc2.SelectSingleNode("response/product/priceGuide/SELL").InnerText.Replace(".", ","));

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Current SELL Price: " + fCardPrice + "\n");

                        fBoxValue += fCardPrice * fFactorMyticBox;

                    }

                    if (xn["rarity"].InnerText == "Uncommon")
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Checking (U): " + xn["enName"].InnerText + "\n");

                        XmlDocument doc2 = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/products/" + xn["idProduct"].InnerText, "GET");

                        float fCardPrice = (float)Convert.ToDouble(doc2.SelectSingleNode("response/product/priceGuide/SELL").InnerText.Replace(".", ","));

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Current SELL Price: " + fCardPrice + "\n");

                        fBoxValue += fCardPrice * fFactorUncommonBox;

                    }
                }
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Calculated Result *: " + fBoxValue + "\n");

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "* Estimated average booster box singles value at MKM SELL Pricing (EUR)\n");

            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.Message);

            }

        }

    }
}
