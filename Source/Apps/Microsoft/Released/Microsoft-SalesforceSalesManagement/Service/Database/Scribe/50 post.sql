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
DROP TABLE dbo.Account;
DROP TABLE dbo.Lead;
DROP TABLE dbo.Opportunity;
DROP TABLE dbo.OpportunityLineItem;
DROP TABLE dbo.OpportunityStage;
DROP TABLE dbo.Product2;
DROP TABLE dbo.[User];
DROP TABLE dbo.[UserRole];
DROP TABLE dbo.Scribe_ReplicationStatus;

/************************************
* Tables to truncate                *
*************************************/
