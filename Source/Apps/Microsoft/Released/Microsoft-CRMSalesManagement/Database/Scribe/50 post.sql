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
DROP TABLE dbo.businessunit;
DROP TABLE dbo.lead;
DROP TABLE dbo.opportunity;
DROP TABLE dbo.opportunityproduct;
DROP TABLE dbo.systemuser;
DROP TABLE dbo.systemusermanagermap;
DROP TABLE dbo.territory;
DROP TABLE dbo.scribe_replicationstatus;
DROP TABLE dbo.product;
/************************************
* Tables to truncate                *
*************************************/
