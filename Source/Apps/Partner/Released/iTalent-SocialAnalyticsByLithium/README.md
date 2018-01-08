Social Analytics for Lithium Solution Template by iTalent Digital Documentation
===========================================================

# Table of Contents
1. [Introduction](#introduction)
2. [Architecture](#architecture)
3. [System Requirements](#system-requirements)
4. [How to Install](#how-to-install)
5. [Architecture Deep Dive](#architecture-deep-dive)
6. [Data Model Schema](#data-model-schema)
7. [Reports Walkthrough](#report-walkthrough)
8. [Estimated Costs](#estimated-costs)



### Introduction

The Social Analytics for Lithium solution template provides analytics about your community, the template stands up an end-to-end solution that pulls the data from Lithium API's, enriches the data for analytics in Azure SQL. Users can then use pre-built Power BI reports to analyze the data and finding insights.
The template is aimed at anyone who is interesting in Lithium community users, boards, categories, topics and replies.

The “Social Analytics for Lithium” by iTalent Digital solution template lets you gain insights about your community by looking at important metrics like total registered users, boards, topics, messages, solutions and kudos. The Power BI Reports provide the community growth in terms of number of users, messages and solution overtime, and analyse board and category level issues and solutions of the product.

The template lets you do things like:

-	Get started quickly with pre-built data models for and advanced community and social analytics reporting.

-	Use an intuitive wizard based UI to deploy, data integration and Power BI Reports.

-	Get insights about your community users and visitors growth and contribution to the community.

-	Go back in history to do trend analysis to see your community growth.

The following document provides a walkthrough of the architecture, a deep dive into every component, comments on customizability as well as information on additional topics like pricing. For any questions not covered in this document, please contact the team at <pbitemplate@italentdigital.com>

### Architecture

![Image](Web/Images/lithiumArchitectureDiagram.png)

The flow of the Social Analytics for Lithium solution template is as follows:

-   Azure Function pulls the data from the Lithium V2 API's

-   Azure Function pushes the data to Azure SQL

-	Azure SQL cleans, transforms and enriches the data for analytics

-   Power BI imports data into it from Azure SQL and renders pre-defined reports


### System Requirements

Setting up the template requires the following:

-   Lithium Community User Account

-   Access to an Azure subscription

-   Power BI Desktop (latest version)

-   Power BI Pro (to share the template with others)


### How to Install

To get started with the solution, navigate to the [Social Analytics for Lithium template page]( https://powerbi.microsoft.com/en-us/solution-templates/) and click **Install Now**.

**Getting Started:** Starting page introducing the template and explaining the architecture.

![Image](Resources/media/gettingstarted.png)

**Lithium Login:** Use OAuth to sign into your community account. You will need to provide your community app TenantID, ClientID, Client Secret and Refresh Access Token (we will not make any changes to your Lithium community account).

![Image](Resources/media/lithiumlogin.png)

You can find TenantID, ClientID, Client Secret details in your community admin settings. To get these details sign into your community and navigate to Community Admin > SYSTEM > API Apps.

![Image](Resources/media/lithiumsettings.png)

*Refresh Token*  Refresh token is required to refresh the access tokens along with other credentials. Access tokens allow the community members to make REST calls with the Community API.  If you already have a refresh token to generate an access to token for your community to make API calls, you can provide that refresh token. If you don't have the refresh token please follow the steps provided [here](https://community.lithium.com/t5/Developer-Documentation/bd-p/dev-doc-portal?section=oauth2#requestAccessRefresh) to generate your refresh key.

**Azure:** Use OAuth to sign into your Azure account. You will notice you have a choice between signing into an organizational account and a Microsoft (work/school account).

If you select a Microsoft account, you will need to provide the application with a domain directory. You can find your domain by logging into your Azure account and choosing from those listed when you click your e-mail in the top right hand corner:

![Image](Resources/media/image3.png)
If you belong to a single domain, simply hover over your e-mail address in the same place:

![Image](Resources/media/image4.png)

In this case, the domain is: richtkhotmail.362.onmicrosoft.com.

![Image](Resources/media/connectazure.png)

Logging into Azure gives the application access to your Azure subscription and permits spinning up Azure services on your behalf. If you want a more granular breakdown of the costs, please scroll down to the Estimated Costs section.

As a user navigates away from this page a new resource group gets spun up on their Azure subscription (the name is random but always prefixed by ‘SolutionTemplate-‘). This name can be changed under the advanced settings tab. All newly created resources go into this container.

**Azure SQL:** Connect to an existing SQL Server or provide details which the application will use to spin up an Azure SQL on your behalf. Only Azure SQL is supported for this template. If a user chooses to spin up a new Azure SQL, this will get deployed in their Azure subscription inside the newly created resource group.

![Image](Resources/media/azuresql.png)

**Summary:** Summary page outlining all the choices the user made.

![Image](Resources/media/summary.png)

**Deploy:** When you navigate to the deployment page the setup process gets kicked off. SQL scripts run to create the necessary tables and views. An Azure Function then gets spun up on your Azure subscription. An Azure ML webservice is deployed to your subscription that will do the sentiment scoring. Finally, a Logic App is created that has a connection to your Azure Function.

**It is important that you do not navigate away from this page while deployment takes place.** Once everything gets deployed a download link will appear for a Power BI file which consists of the pre-defined reports.

![Image](Resources/media/progress.png)

**Power BI Report:** Once you download the Power BI desktop file you will need to connect it to your data. Open the pbix and follow the instructions on the front page.

![Image](Resources/media/downloadreport.png)


### Architecture Deep Dive

The following section will break down how the template works by going through all the components of the solution.

![Image](Web/Images/lithiumArchitectureDiagram.png)

**Azure Resources:**
You can access all of the resources that have been spun up by logging into the Azure portal. Everything should be under one resource group (unless a user was using an existing SQL server. In this case the SQL Server will appear in whatever resource group it already existed in).

![Image](Resources/media/azureresources.png)


Here is an example of what gets spun up for a user. We will go through each of these items one by one:

![Image](Resources/media/azurefunction.png)

**Azure Function:**
Azure functions are serverless compute service that enables you to run code on-demand without having to explicitly provision or manage infrastructure. These functions will run a script or piece of code in response to a variety of events. 

A time trigger azure function will be created during the solution template deployment and schedule to run everyday at 6.00 AM (this can be changed from function schedule settings) in a newly create azure resources in your azure subscription. 


The Run method in the "LithiumETL" azure function calls the LoadandProcessLithiumData method with Lithium credentials and Azure SQL connection string in iTalent.LithiumConnector.GetLithiumData class. 

```C#
public static void Run(TimerInfo myTimer, TraceWriter log)
{
    string strTenantID = ConfigurationManager.AppSettings["LithiumTenantId"].ToString();
    string strClientID = ConfigurationManager.AppSettings["LithiumClientId"].ToString();
    string strClientSecret = ConfigurationManager.AppSettings["LithiumClientSecret"].ToString();
    string strRefreshToken = ConfigurationManager.AppSettings["LithiumRefreshToken"].ToString();
    string strSQLConn = ConfigurationManager.AppSettings["SqlConnectionString"].ToString();

    if (strSQLConn != string.Empty && strTenantID != string.Empty && strClientID != string.Empty && strClientSecret != string.Empty && strRefreshToken != string.Empty)
    {
        log.Info($"Starting the Lithium ETL execution at: {DateTime.Now.ToString()}");

        new GetLithiumData(log).LoadandProcessLithiumData(strSQLConn, strTenantID, strClientID, strClientSecret, strRefreshToken);

        log.Info($"Completed the Lithium ETL execution at: {DateTime.Now} .");
    }
}


The iTalent.LithiumConnector API contains all the methods required to pull the data from Lithium V2 API's using the credential provided and pushes the data into Azure SQL passed to the method.

The API pulls all users, user badges, boards, categories, last 30 days of messages and kudos from Lithium community in JSON format and pushes the data into Azure SQL staging tables. It also pulls the community name and inserts into it.Parameters table in the database. Subsequnt runs will pull only last one day of messages along with all other feed data.

```Messages JSON
{
	"type": "message",
	"id": "300022",
	"subject": "Lithium Solution template",
	"board": "KnowledgeBase",
	"topic": "Lithium Solution template",
	"post_time": "2017-02-10T02:00:00.000-07:00",
	"is_solution": false,
	"metrics": {
	"type": "message_metrics",
	"views": 1000
	}
},
{
    "type": "message",
    "id": "300023",
    "subject": "Create a new community",
    "board": "KnowledgeBase",
    "topic": "Create a new community",
    "post_time": "2017-02-10T02:00:01.088-07:00",
    "is_solution": false,
    "metrics": {
    "type": "message_metrics",
    "views": 2100
	}
},

After all the Lithium data pushes to the Azure SQL staging table, the API calls the it.SyncData stored procedure in the database to merge all the data into actual reporting tables.  To learn more about the schema please go to the Data Model Schema section.


### Data Model Schema

Here is an overview of the tables found in the model:

| **Table Name**       |   **Description**                                                                                                                                                                                                                                             |
|----------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Badges               | A badge is a type of visual reward that community members can earn for completing specific community actions or for achieving important community milestones																																																																		  |
| Boards               | A board is the parent of a conversation (a thread of topic messages and replies). Boards can be contained in categories to provide a structure for your community, although a board can live outside of a category as well.
																										    |
| Categories           | Categories are the highest-level nodes of your community. Depending on the business purpose for your community, your categories might reflect lines of business, product lines, or other high-level divisions.
																										    |
| Kudos                | Kudos are a way for users to give approval to content they like. A kudo boosts the value of a post and improves the reputation of its author.
																										    |
| Messages             | The messages collection represents any kind of post made to the community using one of the Lithium conversation styles. It can represent:
							•	a forum post (a topic or reply)
							•	a knowledge base article or a comment about the article
							•	a question or answer posted to a Q&A board, or any associated comment
							•	a blog article or a comment on the article
							•	an idea submitted to an Idea Exchange or any comment on an idea
							•	a contest entry or comment on an entry
							•	a topic posted to a group discussion board or any associated reply
							•	a product review or any associated comment
																										    |
| UserBadges           | Represents Badges earned by the user.
																										    |
| Users                | The users’ collection represents registered community users. Users exist within a single community and do not cross over between communities.
																										    |


Below is a breakdown of the columns found in every table:

| **Badges**               |                                                        |
|--------------------------|--------------------------------------------------------|
| **Column Name**          | **Description**                                        |
| BadgeId                  | Unique value for the badge table                       |
| BadgeTitle	           | Name of the badge                                      |
| BadgeIconUrl	           | URL of the badge icon image uploaded in the Community  |
| ModifiedBy	           | User name who runs the solution template (system user) |
| ModifiedDate	           | Data row updated/inserted date                         |

| **Boards**               |                                                        
																												|
|--------------------------|--------------------------------------------------------
																												|
| **Column Name**          | **Description**                                        
																												|
| BoardID                  | The unique ID of the resource as defined in Community Admin                       
																												|
| BoardTitle	           | The title of the board as defined in Community Admin
																												|
| ConversationStyle	       | The conversation style of the board. Possible values:
								•	blog - a blog
								•	contest - a contest board
								•	forum - a forum board (discussions)
								•	idea - an idea exchange board
								•	qanda - a Q&A board
								•	tkb - a knowledge base
																												|
| ParentCategoryID         | The parent category of this board, i.e. the category in which the board appears in the community structure. The value is null if the board is	located directly under the community (not within any category). This field must be explicitly set to null when creating/updating a board to	signify that the board belongs at the community (root) level.			
																												|
| IsHidden				   | Whether or not this board is hidden from view in the community UI. Boards can be hidden from lists and menus by selecting Edit Properties in Community Admin > Community Structure.
																												|
| BoardMessages            | Messages count in the board (topics and comments/replies) 
																												|
| BoardTopics			   | Topic messages count (i.e. the root messages) within a board
																												|
| BoardViews			   | The number of views for the board
																												|
| BoardDepth			   | The depth of the board in the community structure
																												|
| ModifiedBy		       | User name who runs the solution template (system user)
																												|
| ModifiedDate	           | Data row updated/inserted date
																												|








### Estimated Costs

The cost of the Social Analytics for Lithium solution template is the total of the costs associated with the Azure resources used therein. Two Azure resources are consumed:

•	Azure Functions

•	Azure SQL Database


**Azure Resources						Monthly Cost**
Azure Functions (Standard Tier)		$75.00
Azure SQL Database (Standard 1)		$30.00


The Lithium connector cost is not included in this document. Its available [here](https://www.lithium.com/company/pricing/price-quote).

Detailed Azure functions costs can be found here [here](https://azure.microsoft.com/en-us/pricing/details/functions/).

Detailed Azure SQL Database service costs are found [here](https://azure.microsoft.com/en-us/pricing/details/sql-database/). The default Azure SQL Database service tier is S1 but can be modified during or after provisioning. If available, an existing Azure SQL Server and Database can be used. 

Power BI costs are not included in this document. The Power BI cost estimator is available [here](https://powerbi.microsoft.com/en-us/pricing/).
