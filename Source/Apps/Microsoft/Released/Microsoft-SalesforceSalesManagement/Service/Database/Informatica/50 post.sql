SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

/************************************
* Tables to drop                    *
*************************************/

-- Scribe needs to recreate these tables, however the fields we needed in the views will still be present
DROP TABLE dbo.account;
DROP TABLE dbo.lead;
DROP TABLE dbo.opportunity;
DROP TABLE dbo.OpportunityLineItem;
DROP TABLE dbo.OpportunityStage;
DROP TABLE dbo.product2;
DROP TABLE dbo.[user];
DROP TABLE dbo.[UserRole];

/************************************
* Tables to truncate                *
*************************************/
