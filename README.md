# Welcome to MKMTool 0.6

## Last changes

27.01.2018 (by Tomas Janak)
+ Added customizable settings for Update Price (see documentation below)
+ Price update no longer uses your own items when calculating the price
+ Prices are not updated if their would change only little (limit is customizable)
+ Richer log options

11.07.2017
+ Fixed weird ug which caused wrong price calculation on foregin systems
+ Fixed crash if articles other than singles are listed on the account

24.04.2017
+ Made GUI respond during article update
+ Error Log improved

08.04.2017
+ changed the default minutes for the bot

23.02.2017
+ country is no longer hardcoded and now determained at startup from your account details
+ code was cleaned up

22.02.2017
+ bug preventing the startup was fixed

## What is this?

MKMTool ist a helper application I wrote for tinkering around with optimization of sale processes on magiccardmarket.eu and the idea of automisation of some tasks most people wouldn’t be able to get done by pure manpower. 

This tool is intended for everyone who is curious about buying and selling MTG cards with more comfort. But needs some (minimum) technical skill at the current state.

First a small disclaimer and things to remember:

+ The entire tool uses the mkmapi 2.0 and is purely designed as proof of concept. If you use this application, always remember that this is coded for fun and not 100% perfectly tested. I am not responsible for any damage or anything it might cause. You are using it in a real environment where real loss of money is possible. Don’t blame me if you sell your lotus for pennies.
+ This is not an official MKM software product, it’s a private venture for the fun of it.
+ Non commercial users have 5000 API calls by day, depending on the function these can be exhausted very quickly (check for cheap deals, get box value i.e.).
+ I am german, the code was written to sell Magic Cards. Everything else will need some adjustments. If you want to sell other things than MTG Singles you need to adjust the code to your needs (Yugi, Pokemon or whatever games I don’t play should be easy, metaproducts might need more love).
+ Beware the program might be slow or if it is calculating it might be not responding a while. This is ok, I didn’t multithread it so this is no problem. Keep an eye on the main log window.
+ In the evening hours if magiccardmarket is crowded, the api seems to be slower, please take this into account if you use the tool.
+ If you find bugs, feel free to report them on github. 
+ This is GPL v3, free software written in C#.

## Installation and starting off

You can simply open the project in Microsoft Visual Studio Community 2015 (free for private use, download URL see at the end of this file) and compile/run it. There is nothing else needed. 

If you are too lazy to compile, here is a build:

http://www.alexander-pick.com/github/MKMTool-05b-Release_11072017.rar 

Before you can use the tool please rename config_template.xml to config.xml and add the apptoken details you can generate in your magiccardmarket profile there. Please note that you need an account which is able to sell to use most of the seller functions.

**Short howto run**

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

![screenshot](http://tomasjanak.github.io/MKMToolUpdatePriceSettings.png)

+ **Minimum price of rares:** This is the minimum price that will ever be assigned to your rares (and mythics) no matter what price is computed.
+ **Minimum # of similar items:** if by the end the sequence of similar items is smaller than this number, no price update will be performed.
+ **Maximum # of similar items:** once the sequence of similar items has this many items, the algorithm will stop adding new ones and will move on to computing the price. Since the sequence is built from cheapest to most expensive items, the larger this number is, the higher the computed price will potentially be as more expensive items will be included. However, it also limits the possibility of the price being too low due to some outliers.
+ **Max price change:** this allows you to set limits to how much can MKMTool change the prices of your cards. If the computed price is too much lower or higher than current price, it will not be sent to MKM. To account for different limits for different prices, this parameter is set as a sequence of pairs of numbers "T1;C1;T2;C2;T3;C3" etc. The first number in each pair, Tx, is a price threshold in €, the second number, Cx, is maximal allowed change in % of current price. For example, on the screenshot above you can see the following sequence: "1;200;5;25;10;15;50;10;100;5;999999;3;".
    
	This means: cards cheaper than 1€ can change by at most 200%, cards cheaper than 5€ by 25%, below 10€ by 15% etc. Let's say you have listed a card A for 0.5€ and card B for 11€. MKMTool will compute new price for card A as 1.4€. This is a 180% change, but that is ok, the price for this level can change by 200%, so anything between 0€ and 1.50€ for card A is good. For price B, it computes 9.35€, i.e. a 15% decrease. Original price is 11€, hence it belongs to the "50;10" pair, i.e. max allowed change 10%. This would be violated, so card B will remain at 11€.
+ **Max difference between consecutive items:** this is a main tool for culling away prices that are outliers. It is recommended to always use this in order to get prices that are agreed upon by majority of sellers and hence are most likely to be describing the real value of the card. The formating is the same as for "Max price change", i.e. sequence of "Tx;Cx" pairs, where Tx is again a threshold, but this time it is a threshold for price of a reference card - such that is already part of the sequence of similar items. Cx is maximal allowed difference (in percent of the price of the reference item) between that item and a next item that should potentially be added to the sequence. 
    
	This is applied in two steps. First, when minimum # of items is reached, the limits are used to cull away outliers on both sides of the sequence, i.e. including suspiciously cheap items: median (item in the middle) of the sequence is chosen as reference and the tool looks at the more expensive item. If it passes the check (it's price is at most [(reference price) + (reference price * Cx/100)]), it is taken as the next reference for the next item etc. If some item is culled, the algorithm ends - it is clear that a sequence of at least min # of items cannot be created and new price cannot be set, because the variance of prices is too large. If none is culled, then starting again from the median, the cheaper items are checked in the same way - this time culling away the too cheap outliers. Then the tool will continue with finiding next similar items. Note that this is done only once - even if some items were culled from the bottom, it is assumed that, for the purpose of outliers culling, the minimum amount of similar items has already been found. For this reason it is recommended to use reasonably large amount of min # of similar items (3 should be absolute minimum) - if you don’t have that, very cheap items will not be culled away.
    
	If the minimum # of items has been found, subsequent items are added only if they pass the difference check, with the last added item being the reference. Once an item that does not pass the test is found (or when maximum # of similar items is found), the algorithm will end and moves on to computing the price.
+ **Set price based on average:** when this is selected, the price will be computed as an average of prices of the similar items. You can also move the slider to go slightly above or below the average, with up to lowest or highest price among the sequence of similar items. The values in-between are computed by a linear interpolation between either the lowest price and average (for the lower half) or average and the highest price.
    
	Example: the sequence has only four items, priced as: 10 12 14 19 €. Average is 13.75€. If you move the slider to half-way between lowest price and average, the price will be set as (Lowest Price + 0.5 * (Average - Lowest Price)), which is 11.875€. On the other side, if you move it half-way between average and highest price, it will be (Average + 0.5 * (Highest Price - Average)), which is 16.375€. Notice the difference is much larger for the second case - that's because the 19€ is quite far away from the other values and in fact should probably be culled away as an outlier (see above).
+ **Set price based on lowest price:** just takes the lowest price and sets new price as a given percentage of it. This is useful if you want to quickly get rid of all your cards, you can ensure that your price will always be lowest by e.g. setting all your cards to 97% of lowest price. However, note that this will not collect the similar items as usual and therefore will not do any culling of outliers. So if somebody for example by mistake lists a card and forgets one '0', you will also end up with unreasonably low price for your card. For this reason, MKMTool will require to set up some "Max price change" limits if you are using this mode. Of course, you can just set some very gratious limits, but don't say we didn't warn you.
+ **Set price based on highest price:** collects the sequence as usual, but instead of computing average, takes the given percentage of the highest price among the sequence and sets it as new price. Useful if you...don't want to sell your cards at all?
+ **Accepted condition:** as mentioned before, MKMTool will always look only at cards that are in the same or better condition as the one you are trying to sell. However, you can control how to treat the better ones. First option is to disregard them completely - only matching condition cards will be used. But this way you can often end up with not enough similar cards. Other extreme option is to not look at the condition at all and go just by the price (right-most option). However, if you are for example selling a damaged card for low price and everybody else has only mint version of this card, the computed price will end up much higher that it should be.
    
	A compromise, and probably the best option, is "Accept better only if there are more expensive matches". Cards in better condition will be accepted as part of the sequence, but in the end, if the last card is not matching, all cards between the last matching and the end of the sequence will be throwed away and price will be determined only based on the remaining cards. This will still not help if nobody is selling cards in similar condition. But in the more usual cases where NM and EXC cards tend to have similar prices, they will be treated as equals and accounted for.
+ **Log updates with significant price change:** when checked, information containing old and new price will be printed in the log for each succesfully updated card. However, the "Minimum price of rares" will be used to determine if you consider the price change significant - when a new price for some item is computed, it is compared to the old price. If the difference is not smaller or higher by the minimum rare price (or more), it will be considered insignificant. This is done so that less items are listed in the output log, so you can easily check only the important changes that were made.
+ **Log updates due to minimal price change:** if the price changed only little, it will not be updated (see "Log updates with significant price change" above). If you want to be notified about such items anyway, check this box.
+ **Log non-updates due to less than minimum # of similar items:** this is tied to the "Minimum # of similar items" parameter. It can be helpful when you are tuning your parameters, mainly the "max allowed difference", to see if you are not culling items away too aggressively.
+ **Log non-updates due to large price change:** this is tied to the "Max price change"  parameter. It is recommended to always leave it turned on - you will be notified about items that spiked in price. However, if you are not updating your prices regularly or you don't have the time to check the log all the time, you might want to simply open up your "max price change" limits.
+ **Log non-updates due to high variance:** this is tied to the "Max difference between consecutive items" parameter, specifically to it's first application to the initial part of the sequence of similar items: if the sequence colletion had to be abandoned because outliers were culled away early, you will be notified about them if this is turned on.
+ **Presets:** since tuning all the parameter can be time consuming, the Preset mechanism allows you to save them once you are happy with them and then re-use them later. Presets are stored in the "Preset" folder as .xml files and contain values for all the settings you can make in MKMTool + a brief description of what the author of the preset wanted to achieve. If you create some interesting preset, you can easily share it with others by simply sending them the xml file (there is a separate file for each preset) and placing it in the "Preset" folder (the app needs to be restarted to load the newly added files). 
    
	When you select a preset in the combo box, you will only be shown its description on the right side. If it sounds good, you can load it by clicking the "Load Preset" button - all the setting parameters will be set to values specified in the preset. Note that there is no undo button, so store your settings first if you want to keep them. You can do that by clicking the "Store Preset..." button. It will open a dialog where you choose the name and description of the preset and then it will store your current settings. You can also delete settings, which will also delete the associated .xml file, so do it only if you are completely sure you want to get rid of it.
    
	The application remembers the last preset you used and automatically loads it when you start it.
+ **Test mode:** if you check this, the computed prices will not be sent to MKM, they will merely be computed and logged for you to check. This is very useful when you are tuning the parameters or when you want to be very careful - you can run first in test mode, see what changes would happen and then, if there is no problem, run it in regular mode.

There is also a Bot mode, you can make the tool execute this task every X minutes. The preset is 360 minutes = 6 hours. Keep your API Limit in mind, the tool needs to request the product data for every article you own to calculate the price, 1 call per item is needed. 

You must feel comfortable with the formula or change it to your demands, be aware that making mistakes in price calculation can cost you real money. Handle with care.

### Check for cheap deals

![screenshot](http://www.alexander-pick.com/github/tool2.PNG)

For the fun of it – with this feature you can find cheap deals with X percent cheaper than other vendors and cheaper than the Trend if selected.- The algo calcs + 1 Eur fixed for shipping to score you a good deal with resale value. It’s also possible to check for cheap deals of cards on your wants list, use this if you are only looking for specific cards. Beware, this feature is API call heavy.

If cards are found they are directly added to your cart on MKM, just log in and check your cart.

### Check Display Value

![screenshot](http://www.alexander-pick.com/github/tool3.PNG)

This is more of an experimental feature, I written it to roughly calculate the expected ROI (return of invest) of a display/box compared to current “real time” MKM SELL prices. Math is a bit clumsy but should be correct, I am always open for improvements to this. I learned that most data published on various sites tends to be rather old or trashy, this gives me a nice view on the “real” site of things. The ROI is shitty, but I still love buying Boxes.

### Want List Editor

![screenshot](http://www.alexander-pick.com/github/tool4.PNG)

Easy Editor for MKM want list, I written this since the MKM features to build a list were too unhandy for me. This works well with the check for cheap deals feature.

## Other Features / Options

### Account Info

Does what it says, shows you the info stack of your account in raw data.

### View Inventory

Pretty much just shows you a list of your currently listed item – nothing special here.

### Update Local MKM Product List
Updates the local offline database provided from MKM. This csv is usually maintained by the program, the option is just for debug purpose.

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



 
