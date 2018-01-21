# Welcome to MKMTool 0.5b

## Last changes

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
The most interesting function (for me at least) is the automatic price update. This function will update all your card sale prices to a match the middle value of the best 4 prices from your country. This will guarantee you a good sale rate by not matching the cheapest seller who is sometimes off and could cause you a bad deal. This could be easily changed to match the 4 best prices by dealers as well, would be a simple code change in the parameters of the call in MKMBot.cs line 234.

Formula is:

sum(Bestprice in your country 1 + Bestprice in your country 2 + Bestprice in your country 3 + Bestprice in your country 3 ) / 4 = your price

*Only offers with same or better condition are checked, also only offers from the same country as you (avoids bad pricing due to foregin low price offers).*

There is also a Bot mode for this, you can make the tool execute this task every X minutes. The preset is 360 minutes = 6 hours. Keep your API Limit in mind, the tool needs to request the product data for every article you own to calculate the price, 1 call per item is needed. 

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



 
