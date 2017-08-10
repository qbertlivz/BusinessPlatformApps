Azure Activity Log Solution Template Documentation
===========================================================

# Table of Contents
1. [Introduction](#introduction)
2. [Architecture](#architecture)
3. [Estimated Costs](#estimated-costs)



### Introduction

The Facebook template provides brand and campaign management for Facebook pages, The template stands up an end-to-end solution that queries the Facebook Graph API, enriches the data using machine learning and stores these enrichments in Azure SQL. Users can then use pre-built Power BI reports that leverage Microsoft research technology to start exploring the data and finding insights.
The template is aimed at anyone who is interesting in following or tracking posts against a Facebook page.
The following document provides a walkthrough of the architecture, a deep dive into every component, comments on customizability as well as information on additional topics like estimated costs. 

For any questions not covered in this document, please contact the team at <PBISolnTemplates@microsoft.com>

### Architecture

![Image](dist/Apps/Microsoft/Released/Microsoft-ActivityLogTemplate/Web/Images/azure-activity-log.png)

The Azure Activity Log pipeline consists of the following resources:

-   Activity Log events are streamed to Event Hub via Log Profiles

-   Event Hub forwards the events to Stream Analytics

-   Stream Analytics processes the data and populates a SQL database

-   Power BI imports data from Azure SQL and renders pre-defined reports


### Estimated Costs

Here is an estimate of the Azure costs (Event Hub, Stream Analytics, Azure SQL) involved:

