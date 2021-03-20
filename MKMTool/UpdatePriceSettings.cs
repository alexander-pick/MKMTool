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
    along with MKMTool.  If not, see <http://www.gnu.org/licenses/>.

    Diese Datei ist Teil von MKMTool.

    MKMTool ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder späteren
    veröffentlichten Version, weiterverbreiten und/oder modifizieren.

    MKMTool wird in der Hoffnung, dass es nützlich sein wird, aber
    OHNE JEDE GEWÄHRLEISTUNG, bereitgestellt; sogar ohne die implizite
    Gewährleistung der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK.
    Siehe die GNU General Public License für weitere Details.

    Sie sollten eine Kopie der GNU General Public License zusammen mit diesem
    Programm erhalten haben. Wenn nicht, siehe <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;

namespace MKMTool
{
  public partial class UpdatePriceSettings : Form
  {
    // price computation based on average - at 0.5, price will be = to average of similar items, at 0
    // it will be equal to the lowest price, at 1 to the highest price. Remaining values are linear interpolation between either 
    // min price and average (0-0.5) or average and highest price (0.5-1)
    private double priceByAvg = 0.5;
    private Dictionary<string, MKMBotSettings> presets;
    private readonly string lastPresetName; // name of the last preset under which it is stored in Properties.Settings.Default
    private readonly PopupListbox allowedExpansionsWindow; // pop-up window managing allowed expansions for filtering
    private readonly PopupListbox allowedCountriesWindow; // pop-up window managing allowed countries for filtering

    /// Initializes a new instance of the <see cref="UpdatePriceSettings"/> class.
    /// <param name="lastPresetName">The last preset used by this settings windows under which it is stored in Properties.Settings.Default</param>
    /// <param name="title">Title of the window</param>
    public UpdatePriceSettings(string lastPresetName, string title)
    {
      InitializeComponent();
      loadPresets();
      Text = title;
      allowedExpansionsWindow = new PopupListbox("Allowed Expansions");
      allowedCountriesWindow = new PopupListbox("Allowed Countries");
      comboBoxPriceEstMinPriceMatch.SelectedIndex = 0;
      List<string> countryNames = new List<string>(MKMHelpers.CountryNames.Count);
      foreach (var code in MKMHelpers.CountryNames)
        countryNames.Add(code.Value);
      allowedCountriesWindow.SetDataSource(countryNames);

      groupBoxPriceGuides.Visible = MKMHelpers.SAmCommercial;
      if (MKMHelpers.SAmCommercial)
      {
        comboBoxPriceEstUpdateMode.Items.Add("Use Price Guides");
        // fill the comboboxes for price guides
        string tooltip = "";
        foreach (var guideDesc in MKMHelpers.PriceGuides)
        {
          comboBoxPriceGuidesFoils.Items.Add(guideDesc.Value.Code);
          comboBoxPriceGuidesNonFoils.Items.Add(guideDesc.Value.Code);
          tooltip += guideDesc.Value.Code + ": " + guideDesc.Value.Documentation + "\n";
          if (guideDesc.Value.Code == "TREND")
            comboBoxPriceGuidesNonFoils.SelectedIndex = comboBoxPriceGuidesNonFoils.Items.Count - 1;
          else if (guideDesc.Value.Code == "TRENDFOIL")
            comboBoxPriceGuidesFoils.SelectedIndex = comboBoxPriceGuidesFoils.Items.Count - 1;
        }
        toolTip1.SetToolTip(comboBoxPriceGuidesFoils, tooltip);
        toolTip1.SetToolTip(comboBoxPriceGuidesNonFoils, tooltip);
        comboBoxPriceGuidesFoils.SelectedIndexChanged += 
          new EventHandler(comboBoxPriceGuidesFoils_SelectedIndexChanged);
        comboBoxPriceGuidesNonFoils.SelectedIndexChanged += 
          new EventHandler(comboBoxPriceGuidesNonFoils_SelectedIndexChanged);
      }
      comboBoxPriceEstUpdateMode.SelectedIndex = 0;

      this.lastPresetName = lastPresetName;
      string lastPreset = Properties.Settings.Default[lastPresetName].ToString();
      if (comboBoxPresets.Items.Contains(lastPreset))
      {
        comboBoxPresets.SelectedIndex = comboBoxPresets.Items.IndexOf(lastPreset);
        UpdateSettingsGUI(presets[lastPreset]);
      }
    }

    /// Instead of closing the settings window when the user presses (X) or ALT+F4, just hide it.
    /// Basically the intended behaviour is for the settings window to act as kind of a singleton object (owned by the main form)
    /// that holds the settings and other parts of the app can read from it.
    /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      base.OnFormClosing(e);
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    /// Gathers data from the GUI and creates their appropriate representation as the MKMBotSettings object.
    /// <param name="s">The settings object after the method successfully executes.</param>
    /// <returns>True if all settings were read, false in case of parsing error.</returns>
    public bool GenerateBotSettings(out MKMBotSettings s)
    {
      s = new MKMBotSettings();

      string[] limits = textBoxPriceEstMaxChange.Text.Split(';');
      double threshold, allowedChange;
      for (int i = 1; i < limits.Length; i += 2)
      {
        if (double.TryParse(limits[i - 1], NumberStyles.Float, CultureInfo.CurrentCulture, out threshold)
            && double.TryParse(limits[i], NumberStyles.Float, CultureInfo.CurrentCulture, out allowedChange))
          s.PriceMaxChangeLimits.Add(threshold, allowedChange / 100); // convert to percent
        else
        {
          MessageBox.Show("The max price change limit pair " + limits[i - 1] + ";" + limits[i]
              + " could not be parsed as a number.", "Incorrect format of max price change format", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      }

      limits = textBoxPriceEstMaxDiff.Text.Split(';');
      for (int i = 1; i < limits.Length; i += 2)
      {
        if (double.TryParse(limits[i - 1], NumberStyles.Float, CultureInfo.CurrentCulture, out threshold)
            && double.TryParse(limits[i], NumberStyles.Float, CultureInfo.CurrentCulture, out allowedChange))
          s.PriceMaxDifferenceLimits.Add(threshold, allowedChange / 100); // convert to percent
        else
        {
          MessageBox.Show("The max difference limit pair " + limits[i - 1] + ";" + limits[i]
              + " could not be parsed as a number.", "Incorrect format of max difference between items", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      }

      s.PriceMinRarePrice = decimal.ToDouble(numericUpDownPriceEstMinPrice.Value);
      s.PriceMinSimilarItems = decimal.ToInt32(numericUpDownPriceEstMinN.Value);
      s.PriceMaxSimilarItems = decimal.ToInt32(numericUpDownPriceEstMaxN.Value);
      if (radioButtonPriceEstPriceByAvg.Checked)
      {
        s.PriceSetPriceBy = PriceSetMethod.ByAverage;
        s.PriceFactor = (double)trackBarPriceEstAvg.Value / (trackBarPriceEstAvg.Maximum - trackBarPriceEstAvg.Minimum);
        s.PriceFactorWorldwide = (double)trackBarPriceEstAvgWorld.Value / (trackBarPriceEstAvgWorld.Maximum - trackBarPriceEstAvgWorld.Minimum);
      }
      else if (radioButtonPriceEstByLowestPrice.Checked)
      {
        s.PriceSetPriceBy = PriceSetMethod.ByPercentageOfLowestPrice;
        s.PriceFactor = s.PriceFactorWorldwide = decimal.ToDouble(numericUpDownPriceEstLowestPrice.Value) / 100;
      }
      else
      {
        s.PriceSetPriceBy = PriceSetMethod.ByPercentageOfHighestPrice;
        s.PriceFactor = s.PriceFactorWorldwide = decimal.ToDouble(numericUpDownPriceEstHighestPrice.Value) / 100;
      }

      s.PriceMarkup2 = decimal.ToDouble(numericUpDownPriceMultCopies2.Value) / 100;
      s.PriceMarkup3 = decimal.ToDouble(numericUpDownPriceMultCopies3.Value) / 100;
      s.PriceMarkup4 = decimal.ToDouble(numericUpDownPriceMultCopies4.Value) / 100;
      s.PriceMarkupCap = decimal.ToDouble(numericUpDownPriceMultCopiesCap.Value);
      s.PriceIgnorePlaysets = checkBoxPricePlaysetIgnore.Checked;

      if (radioButtonCondMatchOnly.Checked)
        s.CondAcceptance = AcceptedCondition.OnlyMatching;
      else if (radioButtonCondAcceptBetterAlways.Checked)
        s.CondAcceptance = AcceptedCondition.Anything;
      else
        s.CondAcceptance = AcceptedCondition.SomeMatchesAbove;

      s.FilterByExpansions = checkBoxFilterExpansions.Checked;
      if (checkBoxFilterExpansions.Checked) // set this only if we are filtering by expansions
        s.AllowedExpansions = allowedExpansionsWindow.GetSelected();

      s.FilterByCountries = checkBoxFilterCountries.Checked;
      if (checkBoxFilterCountries.Checked) // set this only if we are filtering by countries
        s.AllowedCountryNames = allowedCountriesWindow.GetSelected();
      s.IncludePrivateSellers = checkBoxFilterPrivSeller.Checked;
      s.IncludeProfessionalSellers = checkBoxFilterProfSeller.Checked;
      s.IncludePowersellers = checkBoxFilterPowerseller.Checked;

      s.LogUpdated = checkBoxLogUpdated.Checked;
      s.LogLessThanMinimum = checkBoxLogMinItems.Checked;
      s.LogSmallPriceChange = checkBoxLogSmallChange.Checked;
      s.LogLargePriceChangeTooLow = checkBoxLogLargeChangeLow.Checked;
      s.LogLargePriceChangeTooHigh = checkBoxLogLargeChangeHigh.Checked;
      s.LogHighPriceVariance = checkBoxLogHighVariance.Checked;

      s.TestMode = checkBoxTestMode.Checked;
      s.SearchWorldwide = checkBoxPriceEstWorldwide.Checked;

      switch (comboBoxPriceEstUpdateMode.SelectedIndex)
      {
        case 0:
          s.PriceUpdateMode = MKMBotSettings.UpdateMode.TOSS;
          break;
        case 1:
          s.PriceUpdateMode = MKMBotSettings.UpdateMode.UpdateOnlyBelowMinPrice;
          break;
        case 2:
          s.PriceUpdateMode = MKMBotSettings.UpdateMode.OnlyEnsureMinPrice;
          break;
        case 3:
          s.PriceUpdateMode = MKMBotSettings.UpdateMode.UsePriceGuides;
          break;
      }

      switch (comboBoxPriceEstMinPriceMatch.SelectedIndex)
      {
        case 0:
          s.MyStockMinPriceMatch = MKMBotSettings.MinPriceMatch.Highest;
          break;
        case 1:
          s.MyStockMinPriceMatch = MKMBotSettings.MinPriceMatch.Best;
          break;
      }

      if (MKMHelpers.SAmCommercial)
      {
        s.SetGuideNonFoil(comboBoxPriceGuidesNonFoils.SelectedItem.ToString(), textBoxPriceGuidesNonFoil.Text);
        s.SetGuideFoil(comboBoxPriceGuidesFoils.SelectedItem.ToString(), textBoxPriceGuidesFoils.Text);
        s.GuideUseTOSSOnFail = checkBoxPriceGuidesTraverseNotFound.Checked;
        s.GuideLogOnFail = checkBoxPriceGuidesLogNotFound.Checked;
      }

      return true;
    }

    /// Updates the GUI controls according to the provided settings.
    /// <param name="settings">The settings to which to set the GUI.</param>
    public void UpdateSettingsGUI(MKMBotSettings settings)
    {
      textBoxPriceEstMaxChange.Text = "";
      foreach (var limitPair in settings.PriceMaxChangeLimits)
        textBoxPriceEstMaxChange.Text += "" + limitPair.Key + ";" + (limitPair.Value * 100).ToString("f2") + ";";

      textBoxPriceEstMaxDiff.Text = "";
      foreach (var limitPair in settings.PriceMaxDifferenceLimits)
        textBoxPriceEstMaxDiff.Text += "" + limitPair.Key + ";" + (limitPair.Value * 100).ToString("f2") + ";";

      numericUpDownPriceEstMinPrice.Value = new decimal(settings.PriceMinRarePrice);
      numericUpDownPriceEstMinN.Value = new decimal(settings.PriceMinSimilarItems);
      numericUpDownPriceEstMaxN.Value = new decimal(settings.PriceMaxSimilarItems);
      if (settings.PriceSetPriceBy == PriceSetMethod.ByAverage)
      {
        radioButtonPriceEstPriceByAvg.Checked = true;
        radioButtonPriceEstByLowestPrice.Checked = false;
        radioButtonPriceEstHighestPrice.Checked = false;
        trackBarPriceEstAvg.Value = (int)(settings.PriceFactor * (trackBarPriceEstAvg.Maximum - trackBarPriceEstAvg.Minimum) + trackBarPriceEstAvg.Minimum);
        trackBarPriceEstAvgWorld.Value = (int)(settings.PriceFactorWorldwide *
            (trackBarPriceEstAvgWorld.Maximum - trackBarPriceEstAvgWorld.Minimum) + trackBarPriceEstAvgWorld.Minimum);
      }
      else if (settings.PriceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice)
      {
        radioButtonPriceEstPriceByAvg.Checked = false;
        radioButtonPriceEstByLowestPrice.Checked = true;
        radioButtonPriceEstHighestPrice.Checked = false;
        numericUpDownPriceEstLowestPrice.Value = new decimal(settings.PriceFactor * 100);
      }
      else
      {
        radioButtonPriceEstPriceByAvg.Checked = false;
        radioButtonPriceEstByLowestPrice.Checked = false;
        radioButtonPriceEstHighestPrice.Checked = true;
        numericUpDownPriceEstHighestPrice.Value = new decimal(settings.PriceFactor * 100);
      }

      numericUpDownPriceMultCopies2.Value = new decimal(settings.PriceMarkup2 * 100);
      numericUpDownPriceMultCopies3.Value = new decimal(settings.PriceMarkup3 * 100);
      numericUpDownPriceMultCopies4.Value = new decimal(settings.PriceMarkup4 * 100);
      numericUpDownPriceMultCopiesCap.Value = new decimal(settings.PriceMarkupCap);
      checkBoxPricePlaysetIgnore.Checked = settings.PriceIgnorePlaysets;

      if (settings.CondAcceptance == AcceptedCondition.OnlyMatching)
      {
        radioButtonCondMatchOnly.Checked = true;
        radioButtonCondAcceptBetterAlways.Checked = false;
        radioButtonCondMatchesAbove.Checked = false;
      }
      else if (settings.CondAcceptance == AcceptedCondition.Anything)
      {
        radioButtonCondMatchOnly.Checked = false;
        radioButtonCondAcceptBetterAlways.Checked = true;
        radioButtonCondMatchesAbove.Checked = false;
      }
      else
      {
        radioButtonCondAcceptBetterAlways.Checked = false;
        radioButtonCondMatchOnly.Checked = false;
        radioButtonCondMatchesAbove.Checked = true;
      }

      allowedExpansionsWindow.SetDataSource(MKMDbManager.Instance.GetAllExpansionNames(true)); // to make sure we are up to date
      checkBoxFilterExpansions.Checked = settings.FilterByExpansions;
      allowedExpansionsWindow.SetSelected(settings.AllowedExpansions);

      checkBoxFilterCountries.Checked = settings.FilterByCountries;
      allowedCountriesWindow.SetSelected(settings.AllowedCountryNames);
      checkBoxFilterPrivSeller.Checked = settings.IncludePrivateSellers;
      checkBoxFilterProfSeller.Checked = settings.IncludeProfessionalSellers;
      checkBoxFilterPowerseller.Checked = settings.IncludePowersellers;

      checkBoxLogUpdated.Checked = settings.LogUpdated;
      checkBoxLogMinItems.Checked = settings.LogLessThanMinimum;
      checkBoxLogSmallChange.Checked = settings.LogSmallPriceChange;
      checkBoxLogLargeChangeLow.Checked = settings.LogLargePriceChangeTooLow;
      checkBoxLogLargeChangeHigh.Checked = settings.LogLargePriceChangeTooHigh;
      checkBoxLogHighVariance.Checked = settings.LogHighPriceVariance;

      checkBoxTestMode.Checked = settings.TestMode;
      checkBoxPriceEstWorldwide.Checked = settings.SearchWorldwide;
      trackBarPriceEstAvgWorld.Enabled = settings.SearchWorldwide;

      switch (settings.PriceUpdateMode)
      {
        case MKMBotSettings.UpdateMode.TOSS:
          comboBoxPriceEstUpdateMode.SelectedIndex = 0;
          break;
        case MKMBotSettings.UpdateMode.UpdateOnlyBelowMinPrice:
          comboBoxPriceEstUpdateMode.SelectedIndex = 1;
          break;
        case MKMBotSettings.UpdateMode.OnlyEnsureMinPrice:
          comboBoxPriceEstUpdateMode.SelectedIndex = 2;
          break;
        case MKMBotSettings.UpdateMode.UsePriceGuides:
          comboBoxPriceEstUpdateMode.SelectedIndex = 3;
          break;
      }
      switch (settings.MyStockMinPriceMatch)
      {
        case MKMBotSettings.MinPriceMatch.Highest:
          comboBoxPriceEstMinPriceMatch.SelectedIndex = 0;
          break;
        case MKMBotSettings.MinPriceMatch.Best:
          comboBoxPriceEstMinPriceMatch.SelectedIndex = 1;
          break;
      }
      if (MKMHelpers.SAmCommercial)
      {
        var index = comboBoxPriceGuidesNonFoils.Items.IndexOf(settings.GuideNonFoil);
        if (index >= 0) // keep the default value if nothing is stored in the settings
          comboBoxPriceGuidesNonFoils.SelectedIndex = index;
        index = comboBoxPriceGuidesFoils.Items.IndexOf(settings.GuideFoil);
        if (index >= 0)
          comboBoxPriceGuidesFoils.SelectedIndex = index;
        textBoxPriceGuidesNonFoil.Text = settings.GuideModsNonFoil;
        textBoxPriceGuidesFoils.Text = settings.GuideModsFoil;
        checkBoxPriceGuidesTraverseNotFound.Checked = settings.GuideUseTOSSOnFail;
        checkBoxPriceGuidesLogNotFound.Checked = settings.GuideLogOnFail;
      }
    }

    private void checkBoxCondMatchOnly_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButtonCondMatchOnly.Checked)
      {
        if (radioButtonCondAcceptBetterAlways.Checked)
          radioButtonCondAcceptBetterAlways.Checked = false;
      }
    }

    private void checkBoxCondAcceptBetterAlways_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButtonCondAcceptBetterAlways.Checked)
      {
        if (radioButtonCondMatchOnly.Checked)
          radioButtonCondMatchOnly.Checked = false;
      }
    }

    private void trackBarPriceEstAvg_ValueChanged(object sender, EventArgs e)
    {
      priceByAvg = (double)trackBarPriceEstAvg.Value / (trackBarPriceEstAvg.Maximum - trackBarPriceEstAvg.Minimum);
      if (priceByAvg == 1)
        labelPriceEstSliderValue.Text = "Max Price";
      else if (priceByAvg > 0.5)
        labelPriceEstSliderValue.Text = "AVG + " + ((priceByAvg - 0.5) * 2).ToString("f2") + " * (Max Price - AVG)";
      else if (priceByAvg == 0)
        labelPriceEstSliderValue.Text = "Min Price";
      else if (priceByAvg < 0.5)
        labelPriceEstSliderValue.Text = "Min Price + " + ((priceByAvg) * 2).ToString("f2") + " * (AVG - Min Price)";
      else
        labelPriceEstSliderValue.Text = "AVG";
    }

    private void trackBarPriceEstAvgWorld_ValueChanged(object sender, EventArgs e)
    {
      priceByAvg = (double)trackBarPriceEstAvgWorld.Value / (trackBarPriceEstAvgWorld.Maximum - trackBarPriceEstAvgWorld.Minimum);
      if (priceByAvg == 1)
        labelPriceEstSliderValueWorld.Text = "Max Price";
      else if (priceByAvg > 0.5)
        labelPriceEstSliderValueWorld.Text = "AVG + " + ((priceByAvg - 0.5) * 2).ToString("f2") + " * (Max Price - AVG)";
      else if (priceByAvg == 0)
        labelPriceEstSliderValueWorld.Text = "Min Price";
      else if (priceByAvg < 0.5)
        labelPriceEstSliderValueWorld.Text = "Min Price + " + ((priceByAvg) * 2).ToString("f2") + " * (AVG - Min Price)";
      else
        labelPriceEstSliderValueWorld.Text = "AVG";
    }

    private void radioButtonPriceEstPriceByAvg_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButtonPriceEstPriceByAvg.Checked)
      {
        trackBarPriceEstAvg.Enabled = true;
        trackBarPriceEstAvgWorld.Enabled = checkBoxPriceEstWorldwide.Checked;
        radioButtonPriceEstHighestPrice.Checked = false;
        radioButtonPriceEstByLowestPrice.Checked = false;
      }
      else
      {
        trackBarPriceEstAvg.Enabled = false;
        trackBarPriceEstAvgWorld.Enabled = false;
      }
    }

    private void radioButtonPriceByLowestPrice_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButtonPriceEstByLowestPrice.Checked)
      {
        numericUpDownPriceEstLowestPrice.Enabled = true;
        radioButtonPriceEstHighestPrice.Checked = false;
        radioButtonPriceEstPriceByAvg.Checked = false;
        // if assigning by lowest price, no sequence of prices is collected, only the first lowest one is used
        numericUpDownPriceEstMinN.Enabled = false;
        numericUpDownPriceEstMaxN.Enabled = false;
      }
      else
      {
        numericUpDownPriceEstLowestPrice.Enabled = false;
        numericUpDownPriceEstMinN.Enabled = true;
        numericUpDownPriceEstMaxN.Enabled = true;
      }
    }

    private void radioButtonPriceEstHighestPrice_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButtonPriceEstHighestPrice.Checked)
      {
        numericUpDownPriceEstHighestPrice.Enabled = true;
        radioButtonPriceEstPriceByAvg.Checked = false;
        radioButtonPriceEstByLowestPrice.Checked = false;
      }
      else
      {
        numericUpDownPriceEstHighestPrice.Enabled = false;
      }
    }

    private void numericUpDownPriceEstMinN_ValueChanged(object sender, EventArgs e)
    {
      // make sure maximum items is not lower than minimum
      if (numericUpDownPriceEstMinN.Value > numericUpDownPriceEstMaxN.Value)
        numericUpDownPriceEstMaxN.Value = numericUpDownPriceEstMinN.Value;
    }

    private void numericUpDownPriceEstMaxN_ValueChanged(object sender, EventArgs e)
    {
      // make sure maximum items is not lower than minimum
      if (numericUpDownPriceEstMaxN.Value < numericUpDownPriceEstMinN.Value)
        numericUpDownPriceEstMinN.Value = numericUpDownPriceEstMaxN.Value;
    }

    /// Loads all setting presets stored as .xml files in /Presets/ folder
    /// and populates the drop-down list with their names.
    private void loadPresets()
    {
      DirectoryInfo d = new DirectoryInfo(@".//Presets");
      FileInfo[] Files = d.GetFiles("*.xml");
      presets = new Dictionary<string, MKMBotSettings>();
      foreach (FileInfo file in Files)
      {
        MKMBotSettings s = new MKMBotSettings();
        try
        {
          XmlDocument doc = new XmlDocument();
          doc.Load(file.FullName);
          s.Parse(doc);
          string name = file.Name.Substring(0, file.Name.Length - 4); // cut off the ".xml"
          presets[name] = s;
          comboBoxPresets.Items.Add(name);
        }
        catch (Exception eError)
        {
          MKMHelpers.LogError("reading preset " + file.Name, eError.Message, false);
        }
      }
      comboBoxPresets.SelectedIndex = comboBoxPresets.Items.Add("Choose Preset...");
    }

    private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxPresets.SelectedIndex >= 0)
      {
        string chosen = comboBoxPresets.SelectedItem.ToString();
        if (presets.ContainsKey(chosen)) // does not in case the chosen object is the dummy "Choose Preset..."
        {
          labelPresetsDescr.Text = presets[chosen].Description;
          buttonPresetsDelete.Enabled = true;
          buttonPresetsLoad.Enabled = true;
        }
      }
      else
      {
        buttonPresetsDelete.Enabled = false;
        buttonPresetsLoad.Enabled = false;
        comboBoxPresets.SelectedIndex = comboBoxPresets.Items.Add("Choose Preset...");
        labelPresetsDescr.Text = "";
      }
    }

    private void comboBoxPresets_DropDown(object sender, EventArgs e)
    {
      if (comboBoxPresets.Items.Contains("Choose Preset..."))
        comboBoxPresets.Items.Remove("Choose Preset...");
    }

    private void buttonPresetsLoad_Click(object sender, EventArgs e)
    {
      string name = comboBoxPresets.SelectedItem.ToString();
      UpdateSettingsGUI(presets[name]);
      Properties.Settings.Default[lastPresetName] = name;
      Properties.Settings.Default.Save();
    }

    private void buttonPresetsStore_Click(object sender, EventArgs e)
    {
      if (GenerateBotSettings(out MKMBotSettings settings))
      {
        SettingPresetStore s = new SettingPresetStore(settings);
        if (s.ShowDialog() == DialogResult.OK)
        {
          string name = s.GetChosenName();
          presets[name] = settings;
          if (comboBoxPresets.Items.Contains(name)) // it already contains it in case of rewriting a preset
            labelPresetsDescr.Text = settings.Description; // so just rewrite the description
          else
            comboBoxPresets.SelectedIndex = comboBoxPresets.Items.Add(name);
          Properties.Settings.Default[lastPresetName] = name;
          Properties.Settings.Default.Save();
        }
      }
    }

    private void buttonPresetsDelete_Click(object sender, EventArgs e)
    {
      string name = comboBoxPresets.SelectedItem.ToString();
      if (MessageBox.Show("Are you sure you want to delete preset '" + name + "'? This cannot be undone.",
          "Delete preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        try
        {
          FileInfo f = new FileInfo(@".//Presets//" + name + ".xml");
          if (f.Exists) // better check, could have been deleted manually during the run
            f.Delete();
          presets.Remove(name);
          comboBoxPresets.Items.RemoveAt(comboBoxPresets.SelectedIndex);
          comboBoxPresets.SelectedIndex = comboBoxPresets.Items.Add("Choose Preset...");
          labelPresetsDescr.Text = "";
        }
        catch (Exception exc)
        {
          MKMHelpers.LogError("deleting preset", exc.Message, true);
        }
      }
    }

    private void checkBoxPriceEstWorldwide_CheckedChanged(object sender, EventArgs e)
    {
      trackBarPriceEstAvgWorld.Enabled = checkBoxPriceEstWorldwide.Checked && radioButtonPriceEstPriceByAvg.Checked;
      checkBoxFilterCountries.Enabled = checkBoxPriceEstWorldwide.Checked;
      if (!checkBoxPriceEstWorldwide.Checked)
      {
        checkBoxFilterCountries.Checked = false;
      }
    }

    private void checkBoxFilterExpansions_CheckedChanged(object sender, EventArgs e)
    {
      buttonFilterExpansions.Enabled = checkBoxFilterExpansions.Checked;
    }

    private void checkBoxFilterCountries_CheckedChanged(object sender, EventArgs e)
    {
      buttonFilterCountries.Enabled = checkBoxFilterCountries.Checked;
    }

    private void buttonFilterExpansions_Click(object sender, EventArgs e)
    {
      // maybe some expansions were added due to update of database
      allowedExpansionsWindow.UpdateDataSource(MKMDbManager.Instance.GetAllExpansionNames(true));
      allowedExpansionsWindow.ShowDialog();
    }

    private void buttonFilterCountries_Click(object sender, EventArgs e)
    {
      // country codes are hard-coded, no need to update them
      allowedCountriesWindow.ShowDialog();
    }

    private void comboBoxPriceEstUpdateMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      // Options are in this order:
      // Full update - TOSS
      // TOSS only below minPrice
      // Only ensure minPrice
      // Use Price Guides
      groupBoxPriceGuides.Enabled = comboBoxPriceEstUpdateMode.SelectedIndex == 3;
      if (groupBoxPriceGuides.Enabled)
        groupBoxTraversal.Enabled = checkBoxPriceGuidesTraverseNotFound.Checked;
      else
        groupBoxTraversal.Enabled = comboBoxPriceEstUpdateMode.SelectedIndex != 2;
    }

    private void checkBoxPriceGuidesTraversalNotFound_CheckedChanged(object sender, EventArgs e)
    {
      // if check changed, it means priceGuides group is enabled == price update mode is Use Price Guides
      groupBoxTraversal.Enabled = checkBoxPriceGuidesTraverseNotFound.Checked;
    }

    private void comboBoxPriceGuidesNonFoils_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxPriceGuidesNonFoils.SelectedItem.ToString().Contains("FOIL"))
      {
        MessageBox.Show("Warning: using a FOIL trend for pricing NON-FOIL cards!",
          "FOIL trend used for NON-FOIL cards", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void comboBoxPriceGuidesFoils_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!comboBoxPriceGuidesFoils.SelectedItem.ToString().Contains("FOIL"))
      {
        MessageBox.Show("Warning: using a NON-FOIL trend for pricing FOIL cards!",
          "NON-FOIL trend used for FOIL cards", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}
