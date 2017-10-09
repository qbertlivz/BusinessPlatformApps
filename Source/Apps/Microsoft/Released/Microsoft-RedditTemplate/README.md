Reddit Solution Template Documentation
===========================================================

# Table of Contents
1. [Introduction](#introduction)


### Introduction

The Reddut template provides brand and campaign management for Reddit posts and comments.  The template stands up an end-to-end solution that queries the Social Gist's Reddit API, enriches the data using machine learning and stores these enrichments in Azure SQL. Users can then use pre-built Power BI reports that leverage Microsoft research technology to start exploring the data and finding insights.
The template is aimed at anyone who is interesting in following or tracking posts against Reddit.
The following document provides a walkthrough of the architecture. For any questions not covered in this document, please contact the team at <PBISolnTemplates@microsoft.com>


The flow of the Reddit solution template is as follows:

-   Azure Function #1 queries Reddit based on your search criteria.
-   Matching posts are placed on [Queue Storage](https://azure.microsoft.com/en-us/services/storage/queues/) for processing.
-   Azure Function #2 processes every comment from each post, enriching the data and writing it to Azure SQL.
-   Azure Function #3 runs periodically and calls AzureML textual analytics suite.
-   Power BI imports data into it from Azure SQL and renders pre-defined reports
