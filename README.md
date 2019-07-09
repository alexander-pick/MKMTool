# Welcome to MKMTool 0.7.0

If you have just updated to 0.7.0 from older version, please delete the "mkmtool.sqlite" file in your bin folder (don't worry, it will be created anew once needed).

## Last changes

version 0.7.0, x.x.2019 (by Tomas Janak)
Bugfixes:
+ Fixed "Check Display Value" using locale-dependent number parsing, which caused the decimal delimiter be ignored if you are using '.' instead of ',', leading to 100x higher prices.
+ Fixed numerous issues in "Check cheap deals", now it works as intended for all three modes (expansion check, user check, want list check)
+ Error logging is now more systematic, you can find all errors in the error_log.txt, separated for each individual run of MKMTool and with more precise description
+ Local database is updated after pressing the update button even when the files already exist on hard drive
+ Want list editor now also displays "metaproducts", that is cards for which you have set the expansion to "any" (it will show "<any>" in the expansion name column). In line with that, it will no longer crash if you open a want list that has only metaproducts.
+ Fixed behaviour of the delete function in Want List editor: when no line is selected, message is displayed; when multiple lines are selected, all of the selected items are deleted.
+ Fixed a minor bug in update price when using worldwide search: if the condition setting "Accept better only if there is at least one more expensive match" was turned and it would cause not enough cards to be found among domestic items, the worldwide search would not happen, now it does.

New/improved features:
+ Added a new module "Price External List" - allows to generate new prices to a list of card loaded from a CSV file and then export it or directly put on sale to MKM. See [documentation](#CSV-Card-Lists-in-MKMTool) below.
+ The main window can now be enlarged
+ The number of API calls you sent to MKM is now displayed on the bottom of the window along the maximum number of calls you are allowed. When the limit is reached, MKMTool will no longer send any requests to MKM until you restart MKMTool or new day has passed (MKM resets the counter on 0:00 CET). The number of calls comes directly from MKM (they send it with each call), so it is 100% reliable. When you have only 50 calls remaining, the text will turn red to warn you.
+ Reworked GUI for Check for cheap deals. Also, for user check you can now filter only cards from the specified expansion rather than checking all cards of the user
+ Added the option to use different price factor for the average price when worldwide search is used for price update (there is a separate slider for it now)
+ Added the option to add markup on estimated price when you have multiple copies of a given card (see documentation below)
+ Added the option to "ignore" the playset flag when updating prices - when turned on, all articles marked as playset will be treated as 4x single card
+ Separated "log non-updated if price change too large" log option into two - you can now select if you want to log these non-updates only when your price is higher or only when it is lower (also fixed it as the non-updates due to high variance was actually checked when this one should have been checked, so the this checkbox wasn't doing anything). Note that this is not backwards compatible with previous presets - when you load your own preset, you need to check the logging options you like and save it again.
+ Added the option to load a list of cards with minimal prices that the Update Price module uses to not go below specified prices for any card you want (see details in the documentation below)
+ Added the option to export your entire inventory to CSV file from the [View Inventory](#View-Inventory) screen.

9.2.2019 (by Tomas Janak)
+ Fixed "Bad Request 400" error when working with more than 100 items (partially by Ramiro Aparicio) after API changes announced on 6.2.2019
+ Mint condition is treated as Near Mint when looking for similar items
+ Added an option to search for similar items worldwide (ignoring your country) if there aren't enough similar items in your country
+ Fixed a bug causing in some cases the last added item to not be considered as matching condition even when it was
+ Comments are no longer removed from the article upon update
+ Added a "check for cheap deals from user" option

27.01.2018 (by Tomas Janak)
+ Added customizable settings for Update Price (see documentation below)
+ Price update no longer uses your own items when calculating the price
+ Richer log options

11.07.2017
+ Fixed weird bug which caused wrong price calculation on foreign systems
+ Fixed crash if articles other than singles are listed on the account

24.04.2017
+ Made GUI respond during article update
+ Error Log improved

08.04.2017
+ changed the default minutes for the bot

23.02.2017
+ country is no longer hard-coded and now determined at startup from your account details
+ code was cleaned up

22.02.2017
+ bug preventing the startup was fixed

## What is this?

MKMTool is a helper application I wrote for tinkering around with optimization of sale processes on magiccardmarket.eu and the idea of automation of some tasks most people wouldn’t be able to get done by pure manpower. 

This tool is intended for everyone who is curious about buying and selling MTG cards with more comfort. But needs some (minimum) technical skill at the current state.

First a small disclaimer and things to remember:

+ The entire tool uses the mkmapi 2.0 and is purely designed as proof of concept. If you use this application, always remember that this is coded for fun and not 100% perfectly tested. I am not responsible for any damage or anything it might cause. You are using it in a real environment where real loss of money is possible. Don’t blame me if you sell your lotus for pennies.
+ This is not an official MKM software product, it’s a private venture for the fun of it.
+ Non commercial users have 5000 API calls by day, depending on the function these can be exhausted very quickly (check for cheap deals, get box value i.e.).
+ I am German, the code was written to sell Magic Cards. Everything else will need some adjustments. If you want to sell other things than MTG Singles you need to adjust the code to your needs (Yugi, Pokemon or whatever games I don’t play should be easy, metaproducts might need more love).
+ Beware the program might be slow or if it is calculating it might be not responding a while. This is OK, I didn’t multithread it so this is no problem. Keep an eye on the main log window.
+ In the evening hours if magiccardmarket is crowded, the API seems to be slower, please take this into account if you use the tool.
+ If you find bugs, feel free to report them on github. 
+ This is GPL v3, free software written in C#.

## Installation and starting off

You can simply open the project in Microsoft Visual Studio Community 2015 (free for private use, download URL see at the end of this file) and compile/run it. There is nothing else needed. 

If you are too lazy to compile, here is a build:

http://www.alexander-pick.com/github/MKMTool-05b-Release_11072017.rar 

Before you can use the tool please rename config_template.xml to config.xml and add the apptoken details you can generate in your magiccardmarket profile there. Please note that you need an account which is able to sell to use most of the seller functions.

**Short how-to run**

+ download the binaries
+ unzip everything to a folder
+ at MKM go to your account -> profile -> get a token for a dedicated (!) app
+ rename config_template.xml to config.xml
+ open config.xml with an editor and put the token details in the file - field names should be self explaining and this is simple xml
+ run the tool - should work!

The first startup takes a bit since 1 mb of data is downloaded and unzipped, but there should be no problem on Win7 or 10 beyond this.

## Ok - ok, but what can MKMTool do?

![screenshot](http://www.alexander-pick.com/github/tool1.PNG)

MKMTool has 4 Main Features:

### Price Update
The most interesting function (for me at least) is the automatic price update. This function will update all your card sale prices, all you need to do is press the Update Price button. The basic idea is to match your prices to the cheapest prices **from your country** (to avoid dealing with different shipping costs). However, there are numerous parameters that can change how exactly is this price computed, accesible through the "Settings" button on the bottom of the window - it is recommended to look at those first before your first run. The implemented algorithm will now be described, but if it is not good enough for you and you can write some C# code, you can modify the MKMBot class directly (look for MKMBot.updatePrices() method).

The base part of the algorithm is finding "similar items". This is a sequence of cards sold by **other** MKM users in **the same country as you** that are the same as the one you are trying to sell. "The same" means they have the same name, expansion, foil/nonfoil, signed/altered and playset/single statuses. Condition is always either the same or better and will be discussed later. Once the sequence is determined, the price is computed based either on the average or on the lowest or highest prices from the sequence. The sequence is always ordered from the cheapest items up. If the algorithm at some points finds out that it does not need another similar item, it stops reading them and just the ones found so far are used to compute the price. Hence the prices will always be a bit skewed towards the cheaper offers.

The following figure shows the settings window. Each of the parameters will now be described in details, going from top to bottom, left to right:

![Update Price Settings](MKMTool/updatePriceSettings.png)

+ **Minimum price of rares:** This is the minimum price that will ever be assigned to your rares (and mythics) no matter what price is computed.
+ **Minimum # of similar items:** if by the end the sequence of similar items is smaller than this number, no price update will be performed.
+ **Maximum # of similar items:** once the sequence of similar items has this many items, the algorithm will stop adding new ones and will move on to computing the price. Since the sequence is built from cheapest to most expensive items, the larger this number is, the higher the computed price will potentially be as more expensive items will be included. However, it also limits the possibility of the price being too low due to some outliers.
+ **Markup for having multiple copies:** if you have more copies of a given card, you can increase the price a little - buyers who want multiple copies will be willing to pay extra when then safe on shipping compared to ordering the cards from multiple sellers one by one. After computing the price, MKMTool will add the specified percentage of the estimate on top of it. There are three levels of markup you can select - for 2 copies, 3 and for 4 or more. To avoid increasing the price way over what the potential buyers save on shipping, you can specify a cap, in euro. If the added amount would be more than this cap, only the cap value is added. 

	For example,  MKMTool computes a price of 5€ for a given card. You have 3 of them in stock and you set the markup for three copies to 10% and cap to 2€. The marked-up price will be 5.50€. Let's assume the same parameters are used, but now we have 3 cards for 50€. The 10% is 5€, which is above the cap, so the final price will be 52€.

	Note that there are several pitfalls: if you have the same card, but in different conditions or languages, these will not be counted as additional copies. If you have a card listed with the "Playset" tag, the 4-of markup will not be applied - but on the other hand, to estimate its price, only other "Playsets" on sale are used, so you should not need the markup. Lastly, this does not account for how many copies the other sellers have - we are basically assuming that all the N-cheapest prices are from sellers who have only 1 copy on sale. This will in most cases be true, but not always and as a result you might end up with higher prices than is competitive if there are many sellers selling multiple copies at low price. If you have suggestions on how this could be taken into account, leave a comment on the issues board.
+ **Max price change:** this allows you to set limits to how much can MKMTool change the prices of your cards. If the computed price is too much lower or higher than current price, it will not be sent to MKM. To account for different limits for different prices, this parameter is set as a sequence of pairs of numbers "T1;C1;T2;C2;T3;C3" etc. The first number in each pair, Tx, is a price threshold in €, the second number, Cx, is maximal allowed change in % of current price. For example, on the screenshot above you can see the following sequence: "1;200;5;25;10;15;50;10;100;5;999999;3;".
    
	This means: cards cheaper than 1€ can change by at most 200%, cards cheaper than 5€ by 25%, below 10€ by 15% etc. Let's say you have listed a card A for 0.5€ and card B for 11€. MKMTool will compute new price for card A as 1.4€. This is a 180% change, but that is ok, the price for this level can change by 200%, so anything between 0€ and 1.50€ for card A is good. For price B, it computes 9.35€, i.e. a 15% decrease. Original price is 11€, hence it belongs to the "50;10" pair, i.e. max allowed change 10%. This would be violated, so card B will remain at 11€.
+ **Max difference between consecutive items:** this is a main tool for culling away prices that are outliers. It is recommended to always use this in order to get prices that are agreed upon by majority of sellers and hence are most likely to be describing the real value of the card. The formating is the same as for "Max price change", i.e. sequence of "Tx;Cx" pairs, where Tx is again a threshold, but this time it is a threshold for price of a reference card - such that is already part of the sequence of similar items. Cx is maximal allowed difference (in percent of the price of the reference item) between that item and a next item that should potentially be added to the sequence. 
    
	This is applied in two steps. First, when minimum # of items is reached, the limits are used to cull away outliers on both sides of the sequence, i.e. including suspiciously cheap items: median (item in the middle) of the sequence is chosen as reference and the tool looks at the more expensive item. If it passes the check (it's price is at most [(reference price) + (reference price * Cx/100)]), it is taken as the next reference for the next item etc. If some item is culled, the algorithm ends - it is clear that a sequence of at least min # of items cannot be created and new price cannot be set, because the variance of prices is too large. If none is culled, then starting again from the median, the cheaper items are checked in the same way - this time culling away the too cheap outliers. Then the tool will continue with finiding next similar items. Note that this is done only once - even if some items were culled from the bottom, it is assumed that, for the purpose of outliers culling, the minimum amount of similar items has already been found. For this reason it is recommended to use reasonably large amount of min # of similar items (3 should be absolute minimum) - if you don’t have that, very cheap items will not be culled away.
    
	If the minimum # of items has been found, subsequent items are added only if they pass the difference check, with the last added item being the reference. Once an item that does not pass the test is found (or when maximum # of similar items is found), the algorithm will end and moves on to computing the price.
+ **Treat playsets as 4 singles cards:** when checked, each article marked as "Playset" (both yours and other seller's cards) is treated as if it was 4 separate cards. Your cards will still be updated with the flag, but for the purpose of analyzing prices, the flag is ignored (of course, the price per unit is used when using playset articles). This also means that the highest markup will be used. Playsets in general don't appear very often, so this way you can make sure that the prices for your playset items will stay up to date. However, unless you use high markup, you will usually end up with lower price.
+ **Allow worldwide search:** when checked and the minimum # of similar items is not found among seller's from your country, the items are processed again, this time ignoring the country of the seller. This is done only if there is not enough similar items without any culling - if the minimum is not reached because of culling, this will not be done. This option ensures that your prices are updated even for items that nobody in your country sells - typically rare cards like foreign foils etc. You can set a different (recommended higher) factor for the price average using the separate slider.
+ **Set price based on average:** when this is selected, the price will be computed as an average of prices of the similar items. You can also move the slider to go slightly above or below the average, with up to lowest or highest price among the sequence of similar items. The values in-between are computed by a linear interpolation between either the lowest price and average (for the lower half) or average and the highest price.    
	Example: the sequence has only four items, priced as: 10 12 14 19 €. Average is 13.75€. If you move the slider to half-way between lowest price and average, the price will be set as (Lowest Price + 0.5 * (Average - Lowest Price)), which is 11.875€. On the other side, if you move it half-way between average and highest price, it will be (Average + 0.5 * (Highest Price - Average)), which is 16.375€. Notice the difference is much larger for the second case - that's because the 19€ is quite far away from the other values and in fact should probably be culled away as an outlier (see above).
+ **Set price based on lowest price:** just takes the lowest price and sets new price as a given percentage of it. This is useful if you want to quickly get rid of all your cards, you can ensure that your price will always be lowest by e.g. setting all your cards to 97% of lowest price. However, note that this will not collect the similar items as usual and therefore will not do any culling of outliers. So if somebody for example by mistake lists a card and forgets one '0', you will also end up with unreasonably low price for your card. For this reason, MKMTool will require to set up some "Max price change" limits if you are using this mode. Of course, you can just set some very gracious limits, but don't say we didn't warn you.
+ **Set price based on highest price:** collects the sequence as usual, but instead of computing average, takes the given percentage of the highest price among the sequence and sets it as new price. Useful if you...don't want to sell your cards at all?
+ **Accepted condition:** as mentioned before, MKMTool will always look only at cards that are in the same or better condition as the one you are trying to sell. However, you can control how to treat the better ones. First option is to disregard them completely - only matching condition cards will be used. But this way you can often end up with not enough similar cards. Other extreme option is to not look at the condition at all and go just by the price (right-most option). However, if you are for example selling a damaged card for low price and everybody else has only mint version of this card, the computed price will end up much higher that it should be.
    
	A compromise, and probably the best option, is "Accept better only if there are more expensive matches". Cards in better condition will be accepted as part of the sequence, but in the end, if the last card is not matching, all cards between the last matching and the end of the sequence will be throwed away and price will be determined only based on the remaining cards. This will still not help if nobody is selling cards in similar condition. But in the more usual cases where NM and EXC cards tend to have similar prices, they will be treated as equals and accounted for.
+ **Log updates with significant price change:** when checked, information containing old and new price will be printed in the log for each succesfully updated card. However, the "Minimum price of rares" will be used to determine if you consider the price change significant - when a new price for some item is computed, it is compared to the old price. If the difference is not smaller or higher by the minimum rare price (or more), it will be considered insignificant. This is done so that less items are listed in the output log, so you can easily check only the important changes that were made.
+ **Log updates due to minimal price change:** if the price changed only little, it will not be updated (see "Log updates with significant price change" above). If you want to be notified about such items anyway, check this box.
+ **Log non-updates due to less than minimum # of similar items:** this is tied to the "Minimum # of similar items" parameter. It can be helpful when you are tuning your parameters, mainly the "max allowed difference", to see if you are not culling items away too aggressively.
+ **Log non-updates due to large price change:** this is tied to the "Max price change"  parameter. It is recommended to always leave it turned on - you will be notified about items that spiked in price. However, if you are not updating your prices regularly or you don't have the time to check the log all the time, you might want to simply open up your "max price change" limits. There are also separate checkboxes for when your price is too high and for when it is too low. For example, if you do not care too much if your articles are overpriced, but you want to make sure you don't miss if some price went up, you can check the "yours is too low" version and keep the "yours is too high" unchecked.
+ **Log non-updates due to high variance:** this is tied to the "Max difference between consecutive items" parameter, specifically to it's first application to the initial part of the sequence of similar items: if the sequence collection had to be abandoned because outliers were culled away early, you will be notified about them if this is turned on.
+ **Presets:** since tuning all the parameter can be time consuming, the Preset mechanism allows you to save them once you are happy with them and then re-use them later. Presets are stored in the "Preset" folder as .xml files and contain values for all the settings you can make in MKMTool + a brief description of what the author of the preset wanted to achieve. If you create some interesting preset, you can easily share it with others by simply sending them the xml file (there is a separate file for each preset) and placing it in the "Preset" folder (the app needs to be restarted to load the newly added files). 
    
	When you select a preset in the combo box, you will only be shown its description on the right side. If it sounds good, you can load it by clicking the "Load Preset" button - all the setting parameters will be set to values specified in the preset. Note that there is no undo button, so store your settings first if you want to keep them. You can do that by clicking the "Store Preset..." button. It will open a dialog where you choose the name and description of the preset and then it will store your current settings. You can also delete settings, which will also delete the associated .xml file, so do it only if you are completely sure you want to get rid of it.
    
	The application remembers the last preset you used and automatically loads it when you start it.
+ **Test mode:** if you check this, the computed prices will not be sent to MKM, they will merely be computed and logged for you to check. This is very useful when you are tuning the parameters or when you want to be very careful - you can run first in test mode, see what changes would happen and then, if there is no problem, run it in regular mode.

There is also a Bot mode, you can make the tool execute this task every X minutes. The preset is 360 minutes = 6 hours. Keep your API Limit in mind, the tool needs to request the product data for every article you own to calculate the price, 1 call per item is needed. 

You must feel comfortable with the formula or change it to your demands, be aware that making mistakes in price calculation can cost you real money. Handle with care.

You can also define a list of minimum prices for which you want to be selling certain cards. For those cards, the Price Update will not give them lower then specified price no matter what the algorithm computes. This can be useful if you for example want to make sure you are not selling certain cards for less than you bought them for, or if you have certain cards you don't really want to sell unless for a high enough price. The list is defined as follows:

Create a CSV (comma separated values) file in the same folder as your binary is, i.e. alongside your config file, called "myStock.csv" (case sensitive). See [CSV card lists in MKMTool](#CSV-Card-Lists-in-MKMTool) for details about format. You do not need to do anything else to turn the feature on - if the file is present, it will be used (you will also see a message in the log window that the list has been found), if not, the update will still be done without it. After MKMTool computes the price for a particular article, it checks if there is an entry in myStock.csv that matches all attributes of that card and if so, the computed price is compared with the MinPrice in the list and if it is lower, the MinPrice is assigned to the item instead. **You do not have to use all the attributes.** In fact, for the purpose of price update, the only required attribute is the MinPrice itself (if a row in the list does not have it filled, it will be ignored), everything else is optional. When a certain attribute is not filled in for a particular card in the list, it is assumed that you do not care about its value and therefore any article will be considered a match according to that particular attribute. Therefore, each entry in the list does not need to represent a particular card, it can be just sort of a template for a card, a "metacard". In the extreme case, you can have just the MinPrice and nothing else. Such metacard will always match with any card so you can use such metacard to set a minimum price for any card in your stock. Note - MKMTool "groups" (hashes) the metacards by the card name, so only metacards that have a matching name are actually tested for a the card that is being updated + all metacards that have no name assigned. So you don't have to be worried that adding a lot of articles in the list would significantly slow MKMTool down.

For example, assume your myStock.csv looks like this:

| Name  | Expansion | Condition | Foil | MinCondition | MinPrice | 
| ----- | --------- | --------- | ---- | ------------ | -------- |
| Lightning Bolt | | | false | GD | 2 |
| Lightning Bolt | Beta | | | | 200 |
| Lightning Bolt | Magic 2011 | NM | true | | 10 |
| | Beta | | | | 20 |

What does this say? It says you have a lot of Lightning Bolts on sale, you don't want to sell them for less than 2€ unless they are in a less then Good condition, you also have some from Beta, which are much more expensive and also one (or maybe more) foil from M11. Additionally, you have a bunch of other Beta cards and while you didn't want to bother writing all of them down, you are sure that even the worst Beta card is worth at least 20€. Now let's see what happens when you start updating the price of your cards. 

First, let's say we are updating price for Jace, the Mind Sculptor from Worldwake. Our first three metacards are for Lightning Bolt, so that does not match. For the fourth rule, the name matches (because we do not care about it), but the second attribute is Beta and that does not match Worldwake, so we have no matching metacard for our Jace and his price will be assigned without any safety net. 

Next let's say we have a near mint Lightning Bolt from M11. MKMTool computes 1.8€ for it and we go to metacard matching. For the first one, it matches the name, it is not foil and the condition is better than GD - so we have a match for everything. We compare the minimal price and see that it is higher than the computed price, so for now we set the price to 2€, but we still have to compare all the other metacards. The next one have a matching name, but not Expansion, so that's a pass. The next matches in name, expansion and condition, but not the Foil attribute, so it will also not be used. Lastly, the last metacard will not match by expansion. We will put our Bolt on sale for 2€.

Next article we have is a Lightly Played Lightning Bolt from Beta. MKMTool computed 186€ as its price. The first metacard will not be matched, because the condition is not good enough, but the next one will be - the Name matches, the Expansion matches and there is nothing else to check. The minimum price is higher, so we bump up the price to 200€. The third metacard does not match by many attributes, but the last one does as the only attribute is Expansion and it is Beta. However, 20 is less then 200 so after comparing the prices we will keep our 200€ price tag and that is what we upload on MKM in the end.


### Check for cheap deals

![Check Cheap Deals](MKMTool/checkCheapDeals.png)

For the fun of it – with this feature you can find cheap deals with X percent cheaper (the first parameter, see figure above) than other vendors and cheaper than the Trend if selected. You can specify a shipping addition to be added to the lowest found price to ensure a good deal. If cards are found they are directly added to your cart on MKM, just log in and check your cart. Use the parameters on the right side of the window to specify what kind of cards you are looking for and how it should be priced. There are three ways to check cheap deals:
+ Whole expansion check: all cards from a given expansion will be checked for cheap deals.
+ User check: just fill in the username and MKMTool will look through all articles from the specified user and check them for cheap deals. By default, all cards, regardless of expansion, are checked. You can uncheck the checkbox above the button to ensure that only cards from the specified expansion will be taken into account.
+ Wantlist check: specify one of your wantlists in the drop-down menu above the button and MKMTool will look for cheap deals for all the cards in the list. Note that in this case, all the "Card parameters" are ignored - all the cards in the wantlist are checked. The "Price parameters" are still used though.


### Check Display Value

![screenshot](http://www.alexander-pick.com/github/tool3.PNG)

This is more of an experimental feature, I written it to roughly calculate the expected ROI (return of invest) of a display/box compared to current “real time” MKM SELL prices. Math is a bit clumsy but should be correct, I am always open for improvements to this. I learned that most data published on various sites tends to be rather old or trashy, this gives me a nice view on the “real” site of things. The ROI is shitty, but I still love buying Boxes.

### Price External List

![Price External List](MKMTool/priceExternalList.png)

This module allows you to set a price not for cards you already have on sale on MKM, but rather any list of cards you have. The aim is to provide as robust interface as possible: the format of the input file must follow the CSV rules described in [CSV card lists in MKMTool](#CSV-Card-Lists-in-MKMTool), but what attributes you have set for your cards in that list is up to you. The module can work with as little as only the name and still give you a reasonable price estimate. Of course, the more details you include (expansions, languages, conditions etc.), the more realistic prices you will get. Once the list is appraised, you can either save it as a CSV file again, or directly put it up for sale on MKM.

As you can see in the screenshot above, the whole process is divided into three sections going top to bottom: Import, Appraise and Export. We will now describe in details what you can do in each of those parts.

**Import:** In order to be able to appraise a card, we have to be able to determine its product ID. The product ID is MKM's internal identification number which is unique for each card from a particular expansion. Cards for which MKMTool is not able to determine this will not be appraised, but they will still be kept internally so that they can be later exported alongside your appraised items (see more in the **Export** section). The following list shows all possible options to do determine the ID based on which card attributes are included in your CSV file. 

1. Include *idProduct*: the most straightforward way. If the product ID is set for the item, the English name and expansion will be fetched from MKMTool's database (which is automatically updated to include all Magic: the Gathering cards) as well. Of course, in most cases, you will not know what the ID is...
2. Include *Name* and *Expansion*: this is doing it the other way around - based on the name and expansion, we can fetch the product ID from our database as well. **However, there are several special cases where a certain card exists multiple times in one expansion. Most commonly it is basic lands, which have several versions, but there are other examples here and there (guildgates in Guild of Ravnica, general cards in some Commander products etc.). In those cases, the only other way we can identify it is if you provide the Card Number. If it is not specified in the list, the card will not be imported and an error will be printed out in the log, asking you to provide the Card Number. If you do, MKMTool will request the product info from MKM API and will select the correct card. You can find the Card Number on the bottom of the card - but only for newer cards. This means that for example old (Tempest block and older) basic lands are simply not possible to identify and you will either have to somehow provide product ID for them or price them manually. One little hack you can do if you do not know which version it is you have, but you also do not care / think they are all similar price, you can look into mkminventory.csv stored in MKMTool's directory. It includes all magic singles - search for yours (from the appropriate expansion) and choose one of the product IDs you find there at random.** 
3. Include *LocName*, *Expansion* and *Language* or *LanguageID*: if you have the non-English name, we can find the product ID as well, but in this case it is necessary to know what the language is. Moreover, non-English names are not stored in MKMTool's local database, so for each item for which we have to use this method, we need to make one API request.

These methods will be used in the order written, i.e. if the data is successfully determined in one way, the later methods are not used and the name, expansion and product ID are all set based on the result of this method. This means that if you for example have *idProduct* set for a given item, the name and expansion will be fetched from MKMTool's database and whatever you might have entered in the *Name* column will be ignored, so it might even be the wrong name (e.g. with a typo) and appraisal will work. But this works the other way around as well - if you enter a wrong product ID, the card will not be processed even if you also entered (correct) name and expansion.

While the 2. and 3. method used expansion and *Language*, you do not actually necessary need them. As you can notice on the screenshot above, the first part of the GUI contains several combo-boxes for assuming attributes of the card when they are not known. This means that if a given attribute is empty in your list, the specified "default" value will be set for that card:
+ For the boolean attributes (foil, signed etc.) you can set "Yes", "No" or "Any". Setting "Any" is the same as not setting anything and means that appraisal should use both cards that have and have not the given property. This could in some cases lead to some strange prices, but in general, "Any" will be very similar to "No" - having properties like foil or signed generally increases the price of the card, so if you are setting the price based on cheapest items, those items would not be used anyway. It is when your card does have the given flag that you should make sure it is set correctly.
+ For condition, you choose the one default. Note that when using the MKMTool to set prices, it treats Mint and Near Mint conditions the same.
+ For language, you can choose either a specific language or "All", which means the language will be ignored and cards from all languages will be considered when setting the price for your card.
+ For expansion, you have several options. First two use the release date of the expansion, you can choose either the oldest or latest one in which the card with the specified name was printed. This is stored in the local database, so it uses not additional API calls. The three other methods are based on MKM's "price guides", see the list of attributes in [CSV card lists in MKMTool](#CSV-Card-Lists-in-MKMTool) for a list of the different prices included in the guide. When you choose to use "Price guide - Low", there are actually several different prices that can be used: if the item is foil, the *LOWFOIL* price is used. If it is not and is in condition EX or better, the *LOWEX* price is used, if it is in worse condition, the *LOW* is used. If you choose "Price guide - Average" and the card is not foil, the *AVG* is used, if it is foil, the *TRENDFOIL* is used (AVG does not account for foil cards so *TRENDFOIL* is probably the most accurate in this case). If you choose "Price guide - Trend", either *TREND* or *TRENDFOIL* is used based on whether your card is foil or not. This will require one API call for each item. However, the price guide is stored and can later be exported and used as your new price.
  There is one caveat with the automatic determination of expansion and that is foil cards. There are some expansion that do not have foil cards, some that have only foil cards (various promo expansions) and of course most modern expansion can have both. However, as of now, there is no direct way to find this property of a given expansion. I have contacted the MKM's support and received a reply on 9.7.2019 that said they will put it on their To-Do list and will try to implement it in the future, but as of now they cannot give me an estimated time when will it be done. Until then, you are running certain risks with appraising cards that were at some pointed printed in foil-only or non-foil-only set. For example, if you have the card Yawgmoth's Will in your list and choose automatic expansion "latest", Price External List will assign the "Judge Rewards" expansion to it, which is a foil-only expansion - even if you specifically set the foil attribute to "false". Later on, when the price is being computed, MKMTool will not find any articles matching the description because it will be finding only foils and so you will get no price for your card even when there are a lot of similar ones on sale. Therefore, if you can avoid it, always try to write down what expansion each card comes from.

Whenever importing some item fails, you will get a message with line number of the card that failed to import. If you check the "Log all imported articles" checkbox, you will also get a message about the successfully imported ones as well.

Once you click the *Import CSV file...*, a file dialog will appear for you to choose the list to import and the import will start once you choose a file. Once completed, the Appraise and Export buttons will become available.

**Appraise:** Once you have imported a list, you can set a price to each (successfully imported) article in it using the Appraise button. Using the two checkboxes above it you can select which price should be computed: 
+ If you check the MKM price guide prices, the price guides will be downloaded from MKM for each article. There are 7 prices MKM provides within the guide (see the **XXX - MKM Price Guide** attribute in [CSV card lists in MKMTool](#CSV-Card-Lists-in-MKMTool)). MKMTool will store *all* the price guides, so even if you article is for example not foil, the price guides for foil items will be included in the export. Note that for articles for which the price guide was fetched during import, no new request will be made to MKM. However, if your import list already includes one or more of the **XXX - MKM Price Guide* attributes, the new price guides will still be downloaded and will overwrite the ones you had in your list.
+ If you check the MKMTool prices, MKMTool will use its algorithm used for [Price Update](#Price-Update) to set the price as the **MKMTool price** attribute. This means price will be set based on similar articles currently on sale using as many details about your card as you provide. Also, it will store the price of the cheapest matching (same condition, language etc.) article on sale as the **Price - Cheapest Similar** attribute. You can choose the settings for the algorithm by clicking the settings button next to the checkbox. It will open the same window settings window as is used for the price update, with the small change that it uses its own history of last preset used, so you can easily use different presets for Price Update and different for Price External List. A "generate new prices" preset is provided as one reasonable preset, but you are encouraged to modify it to fit your pricing strategy. It is recommended to keep the "variance" settings generous and very low "minimal number of similar items" in order to almost always get some price. Also, since you are setting a brand new price, the "maximum allowed price change" parameter is ignored. In fact, internally Price External List assumes that the current price of the card being appraised is -9999€, which is a number you will see during logging in the main window as the appraisal is done. Also it means that in the log options, if you turn on "Log updates with significant price change", all updates will in fact be logged, since all will be very large compared to the -9999€.

**Export:** There are two options for export, to CSV and "upload to MKM", which means the appraised articles will be put on sale on your MKM profile. Before describing the details of the two methods, note that you technically do not need to do appraisal in order to be able to do export. For example, you can import a list which already has pre-filled column with prices and directly upload it to MKM. Or maybe you have a list of card names in non-English language and you just want to translate them to English, you can import and then immediately export with all the extra information that was generated during import.

Export to CSV is very simple, you just press the button, choose a file to overwrite or a new one and the list will be saved. All the columns that were in the imported file will be kept and in the same order. If you want to export the generated prices (which you probably do), check the "MKMTool prices" and/or "MKM price guide prices". This will add columns with the selected additional attributes. If you check the "Include all known info", every attribute that was generated during the process will be written out - product ID, language ID, all the boolean flags etc.

There are several online services that allow you to track your collection. As of version 0.7., MKMTool is able to export (and import) data in a format compatible with one of such sites - www.deckbox.org. However, since there are some differences between formats, you have to check the "Force deckbox.org format" if you want the exported list to be directly upload-able to deckbox.org. Specifically, the Foil, Signed, Altered and Condition will have different values (see [CSV card lists in MKMTool](#CSV-Card-Lists-in-MKMTool) for more details, especially for Condition as that includes some assumptions about how are the MKM conditions translated into the ones used by deckbox.org). Also, if you use the "Include all known info", you will have to delete some columns as deckbox.org will see them as duplicated: you might end up with having both "Expansion" and "Edition" (delete any one of them), delete LanguageID (for some reason deckbox.org sees it the same as Language) and maybe others, the deckbox' web interface will tell you which columns are duplicated.
If you are interested in adding the support for some other format, leave a suggestion on the issues board.

The last option for export is "Only appraised". When this is not checked, all the articles in the exported list will exactly match the ones in the imported one line by line. That means that even articles that were either not successfully imported, or not successfully appraised will still be there. If you check "Only appraised", only the ones that had at least one price generated for them will be exported.

For the second export mode, Upload to MKM, all you have to do is set which price should be used as the price for which you will be selling the article and then press the button and wait a little while. You can check the "Log all uploaded articles" to see what articles and at what price are being uploaded, otherwise you will see only the ones that either failed to be uploaded or did not have a valid price set to them. Regarding what price is used, you can choose any of the two MKMTool prices, i.e. the cheapest similar item or the full MKMTool price. Alternatively you can choose one of the price guides, using the same logic as described above in the **Import** section:
"Price guide - Low" is LOWFOIL for foils, LOWEX for non-foil in EX+ condition, LOW for worse conditions; "Price guide - Average" is TRENDFOIL for foils and AVG for non-foils; and "Price guide - Trend" is TRENDFOIL for foils and TREND for non-foils. In any case, it is recommended to at least check the prices first by exporting to CSV and looking at them - the price guide prices are often very inaccurate, especially for articles that do not get sold very often, i.e. old expensive cards.

In order to be able to upload a given card to MKM, it has to have a set Language - although it is possible to use "All" as the default language and MKMTool will be able to appraise such item, you must specify some language to be able to upload it to MKM. Also keep in mind that any boolean flags set to "Any" will be interpreted as "false" by MKM.

### Want List Editor

![screenshot](http://www.alexander-pick.com/github/tool4.PNG)

Easy Editor for MKM want list, I written this since the MKM features to build a list were too unhandy for me. This works well with the check for cheap deals feature. However, the functionality is (in the current version) limited: for language, you have to choose either all, or one specific language, not a combination of several languages; you have to choose one precise condition; for boolean flag as foil, signed etc., you can choose only yes or no, not "any"; for expansions, you have to choose a specific one, you can't choose "any"; similarly, if you have chosen "any" expansion in the MKM's want list editor, MKMTool will display it only as "<any>", it won't list all the possible expansions.

## Other Features / Options

### Account Info

Does what it says, shows you the info stack of your account in raw data.

### View Inventory

Pretty much just shows you a list of your currently listed item – nothing special here.

### Update Local MKM Product List
Updates the local offline database provided from MKM. This csv is usually maintained by the program, it automatically updated if it is older than 100 days or when some module attempts fails to find the data it is asking for (and the database is more than 1 day old). So most of the time, you should not need to click this button, except in cases when for example new expansion just comes out and you want to use the "Check for Cheap Deals" option, updating the database will make the new expansion appear in the list of expansions.

### CSV Card Lists in MKMTool
There are several modules in MKMTool that use import or export from/to Comma Separated Value (CSV) files. Each module has some specific to which card attributes are or are not expected to be in the list, but the general format is the same and described below.
To create the file, it is recommended to use your favourite spreadsheet software, all of them should have an option to save as CSV. However, there are several viable formats, so ensure that the one your spreadsheet software is using follows these rules:
+ The separator is comma (i.e. the character ','). This can usually be easily changed in the export settings.
+ All fields that contain a comma as part of the value are enclosed in double quotation marks (i.e. the character '"'). For example, if a text in some cell of the spreadsheet is *Jace, the Mind Sculptor*, when you save it as CSV and open in an ordinary text editor, you should see *"Jace, the Mind Sculptor"*. Usually the spreadsheet software will do this automatically. Fields that do not contain a comma may be, but do not have to be, also enclosed in quotation marks.
+ If there is a double quotation mark as part of the value in some field, it has to be doubled. Again, most spreadsheet softwares will do this automatically, but it can depend on the particular CSV format, so it is recommended to test this and see if you are getting the correct result when you open the file in a regular text editor (such as Notepad). For example, if you have a field with *"Ach! Hans, Run!"*, it should be exported as: *"""Ach! Hans, Run!"""*. Note that this not only doubled the quotes in the value, but also added quotes around the text, because it contains a comma.

The first line of the file must be a header naming all the columns - the *attributes* of the cards. Below you will find a list of all attribute names that are *recognized* by MKMTool. However, your file may contain other columns as well, they will simply be ignored. The attribute may also be in any order. If there are multiple attributes with the same names, only the latest will be used. Some attribute names have synonyms - for example, your column can be named "Name" or "enName", it will be understood the same way. If you have two (or more) attributes that are synonyms, both will be kept, but keep in mind that MKMTool internally works only with the "main" name of the attribute, so if you for some reason have different values in the two columns, only the "main" one will count, the other will be kept along, but never used internally.

The following is the list of all recognized attributes. **Note that all of the attribute names are case sensitive and many of the values are as well**:

+ **Name**: the name of the card, in English. Case sensitive, use the name exactly as you can find it on MKM's product page, i.e. first letters capitalized except for prepositions. Synonyms: enName.
+ **LocName**: the name of the card in the language in which it is printed. Note that different languages have different rules about the capitalization - some are like English, but some have only first letter of the first word capitalized.
+ **Language**: English name of the language, first letters capital, i.e. one of the following: English; French; German; Spanish; Italian; Simplified Chinese; Japanese; Portuguese; Russian; Korean; Traditional Chinese
+ **LanguageID**: MKM's language ID, an integer with values 1-11: 1 for English, 2 for French, 3 for German, 4 for Spanish, 5 for Italian, 6 for Simplified Chinese, 7 for Japanese, 8 for Portuguese, 9 for Russian, 10 for Korean, 11 for Traditional Chinese.
+ **Expansion**: the expansion of the card. Case sensitive. Use the name in exactly the form you can find on MKM product pages, i.e. first letters capitalized except for prepositions. If it is assigned, MKMTool will also internally fill in the **Expansion ID** field. Synonyms: Edition.
+ **Expansion ID**: MKM's identification number of expansion. If it is assigned, MKMTool will also internally fill in the **Expansion** field.
+ **Condition**: condition of the card. Preferred is the condition used by MKM entered as a two-letter abbreviation (case insensitive): MT for Mint, NM for Near Mint, EX for Excellent, GD for Good, LP for Light Played, PL for Played, PO for Poor. You can also use the "American" grading you would get if you for example export your collection from deckbox.org. Each condition is full word (case insensitive): Mint, Near Mint, Good (Lightly Played), Played, Heavily Played, Poor. You can also mix the two. However, in the end, MKMTool will internally translate everything to the MKM system and as you can notice, the "American" grading has one less grade. The translation is done as such: Mint = MT, Near Mint = NM, Good (Lightly Played) = EX, Played = GD, **Heavily Played = LP**, Poor = PO. This means that if you import your cardlist from deckbox.org (or other site using the same grading), none of your cards will ever be considered as PL by MKMTool. If that is a problem for you, it is recommended you use the MKM grading, at least for the problematic items. Also note that the grade "Good (Lightly Played)" is understood as EX only when the whole string is written as such. If you write just "Good", it will be understood as GD, if you write just "Lightly Played", it will be understood as LP.
+ **MinCondition**: minimal condition of the card. Same rules apply as for **Condition**, but the comparisons for the purposed of matching is done by checking the cards MinCondition against other cards Condition.
+ **Foil**: whether it is foil or not. Valid values are (all case insensitive): "true" or "foil" for foil cards, "false" for non-foil cards, "null" or empty for when you do not care.
+ **Signed**: whether it is signed or not. Valid values are (all case insensitive): "true" or "signed" for signed cards, "false" for non-signed cards, "null" or empty for when you do not care.
+ **Altered**: whether it is altered or not. Valid values are (all case insensitive): "true" or "altered" for altered cards, "false" for non-altered cards, "null" or empty for when you do not care. Synonyms: Altered Art.
+ **Playset**: whether it is a playset or not. Valid values are (all case insensitive): "true" for playsets, "false" for non-playsets cards, "null" or empty for when you do not care. Note that for playsets, the MinPrice is the price for the entire playset, not a single unit, regardless of whether you are using the "treat playsets as single cards" option or not.
+ **idProduct**: MKM's identification number of the product. If it is assigned, MKMTool will also internally fill the Name, Expansion and Expansion ID fields. You can still have them in the list, but MKMTool will use the ones found based on this ID. 
+ **idMetaproduct**: MKM's identification number of the metaproduct this product belongs to. Metaproducts gather all cards with the same name, but from different expansions.
+ **Count**: the number of the items. It is recommended *not* to use this in you myStock.csv file. If you do, the match will be required. So if you for example have 5x some card, you write the count down in the list, then you sell one later, you will no longer get a match next time you do price update unless you manually update the Count in the myStock.csv.
+ **Rarity**: rarity of the card, full word, case sensitive. Valid values are: Masterpiece, Mythic, Rare, Special, Time Shifted, Uncommon, Common, Land, Token, Arena Code Card, Tip Card.
+ **MinPrice**: the minimal price MKMTool will use when updating prices, in €.
+ **MKMTool price**: the price assigned to the article by MKMTool, in € (not relevant for Price Update, should not be used for myStock.csv).
+ **Price - Cheapest Similar**: the price of the cheapest article currently on sale with the same attributes (not relevant for Price Update, should not be used for myStock.csv).
+ **Price**: the price of the article on MKM. Note that technically this is pointless for the Price Update module and you should not include it in your myStock.csv as it will cause you to get a match on the metacard only if your current price is exactly equal to this number.
+ **Comments**: the message stored as an MKM comment for the article.
+ **Card Number**: Number of the card within the expansion.
+ **XXX - MKM Price Guide**: Not recommended to use for myStock.csv. This is several fields that include the price guides MKM includes with their "product" entries from the API. Note that when MKMTool sets the price guide based on a product entry obtained from MKM API, it stores the entire guide - non-foil cards will include the guide for foil cards and vice versa, cards worse than EX will have LOWEX+ price guide etc. XXX can be one of the following:
   - SELL                   // Average price of articles ever sold of this product
   - LOW                    // Current lowest non-foil price (all conditions)
   - LOWEX                  // Current lowest non-foil price (condition EX and better)
   - LOWFOIL                // Current lowest foil price
   - AVG                    // Current average non-foil price of all available articles of this product
   - TREND                  // Current trend price
   - TRENDFOIL              // Current foil trend price

As was mentioned several times in the list above, the format is done in such a way to be compatible with card lists you might export from various other tools for collection management. Currently it is compatible with deckbox.org, if you are using other tools and their export format is not directly compatible, leave a message on MKMTool's issue board and if it is simple enough to incorporate the format, *someone* might do it.

### Resource URLS

Here are some URLs I thing you need to know about:

+ MKM International site: https://www.magiccardmarket.eu/
+ MKM German site: https://www.magickartenmarkt.de/
+ MKM API documentation (MKMTool uses v2.0): https://www.mkmapi.eu/ws/documentation
+ MS Visual Studio 2015 Community: https://www.visualstudio.com/de/downloads/

### And finally

The development of this tool cost me a lot of time recently and a few bad (luckily bulk card) sales during the adjustment of the algorithm but it was worth it to see it done now. 

If you like this tool, you can donate me some bitcoin leftovers to my wallet here:

13Jjvvnmn6t1ytbqWTZaio1zWNVWpbcEpG

Or by me something of my amazon wishlist here:

https://www.amazon.de/registry/wishlist/PY25W6O71YIV/ref=cm_sw_em_r_mt_ws__ugkRybY0HFNYD

*Sorry I don’t have paypal.*

**If you are producing a commercial product and need some help with the MKM API or you want to integrate some of my code in your application, feel free to contact me.**



 
