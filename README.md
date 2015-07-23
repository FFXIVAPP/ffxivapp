# FFXIVAPP

Issue tracking, feature request and release repository.

[![Join the chat at https://gitter.im/Icehunter/ffxivapp](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Icehunter/ffxivapp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Main Application Installation
<a href="http://youtu.be/jXhwvqe45MI" target="_blank">
    <img src="http://i1.ytimg.com/vi/jXhwvqe45MI/mqdefault.jpg" />
</a>

## Recommended Format (Issues)
**Title**:
*brief description*

**Description**:
Detailed description including any log lines or screenshots to illustrate the issue.

## Recommended Format (Suggestions)
**Title**:
As a **<who>** (I can/want to) **<quick description>** (because/so I can) **<what>**.

**Description**:
This is where you can provide examples of what you mean, provide illustrations to support the request (if needed or helpful) and to say if you want; what you ideas of "definition of done" would be.

## Formatting
Formatting can be achieved via example from the follow URL: [Markdown](http://daringfireball.net/projects/markdown/)

## Open-Source Plugins

* http://github.com/icehunter/ffxivapp-plugin-event
* http://github.com/icehunter/ffxivapp-plugin-informer
* http://github.com/icehunter/ffxivapp-plugin-log
* http://github.com/icehunter/ffxivapp-plugin-radar
* http://github.com/icehunter/ffxivapp-plugin-parse
* http://github.com/icehunter/ffxivapp-plugin-widgets

Directions on how-to install for FFXIVAPP are found on each repo. Thanks!

## Plugin Installation

Plugins are NOT included in this download. When you open the application go to the Update tab and install the plugins you want; or update one if an update is available.

If you are a developer and want to distribute your own plugins and have update availability then you must have them hosted online somewhere. You will need to make a VERSION.json file that can be added to a sources list in the application.

The format of the file is as follows:

``` javascript
{
    "PluginInfo": {
        "Name": "Plugin.Name.dll",
        "Version": "3.0.5145.29167",
        "Files": [
            {
                "Name": "Plugin.Name.dll",
                "Location": ""
            },
            {
                "Name": "Logo.png",
                "Location": ""
            },
            {
                "Name": "PluginInfo.xml",
                "Location": ""
            }
        ],
		"SourceURI": "http://url/to/base/directory/of/download"
    }
}
```

If you have other files they can be added into that structure. If they are in sub-directories the the location would be something like this:

``` javascript
{
    "PluginInfo": {
        "Name": "Plugin.Name.dll",
        "Version": "3.0.5145.29167",
        "Files": [
            {
                "Name": "Plugin.Name.dll",
                "Location": ""
            },
            {
                "Name": "Logo.png",
                "Location": ""
            },
            {
                "Name": "PluginInfo.xml",
                "Location": ""
            },
			{
				"Name": "Some.ext",
				"Location": "path/to/sub/folder"
			}
        ],
		"SourceURI": "http://url/to/base/directory/of/download"
    }
}
```

The location will not have a leading or trailing slash.
