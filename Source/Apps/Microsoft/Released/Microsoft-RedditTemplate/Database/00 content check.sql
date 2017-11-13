SELECT Count(*) AS ExistingObjectCount
FROM   INFORMATION_SCHEMA.TABLES
WHERE  ( table_schema = 'reddit' AND
            table_name IN (
				'Documents', 
				'Sentiment', 
				'Posts', 
				'Comments', 
				'KeyPhrases', 
				'EmbeddedUrls', 
				'Entities', 
				'UserDefinedEntities', 
				'UserDefinedEntityDefinitions', 
				'Staging_Sentiment', 
				'Staging_Entities', 
				'Staging_KeyPhrases'
			)
        );
