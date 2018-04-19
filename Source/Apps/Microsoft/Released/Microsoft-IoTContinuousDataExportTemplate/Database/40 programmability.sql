SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
GO

CREATE PROCEDURE [dbo].[TransformDevices] 
	@PreviousChangeTrackingVersion BIGINT, 
	@CurrentChangeTrackingVersion BIGINT
AS
BEGIN
    MERGE analytics.Devices
    USING (
        SELECT
            stage.Devices.deviceId,
            (stage.Devices.modelId + '/' + stage.Devices.modelVersion) as model,
            stage.Devices.name,
            stage.Devices.description,
            stage.Devices.simulated
        FROM stage.Devices
        INNER JOIN CHANGETABLE(CHANGES stage.Devices, @PreviousChangeTrackingVersion) AS CT
        ON CT.id = stage.Devices.id
        WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
    ) AS changes (deviceId, model, name, description, simulated)
    ON analytics.Devices.deviceId = changes.deviceId
    WHEN MATCHED THEN
        UPDATE SET
            analytics.Devices.model = changes.model,
            analytics.Devices.name = changes.name,
            analytics.Devices.description = changes.description,
            analytics.Devices.simulated = changes.simulated
    WHEN NOT MATCHED THEN
        INSERT (deviceId, model, name, description, simulated)
        VALUES (changes.deviceId, changes.model, changes.name, changes.description, changes.simulated)
    ;
END
GO


CREATE PROCEDURE [dbo].[TransformMeasurementDefinitions] 
	@PreviousChangeTrackingVersion BIGINT, 
	@CurrentChangeTrackingVersion BIGINT
AS
BEGIN
    MERGE analytics.MeasurementDefinitions
    USING (
        SELECT
            (stage.MeasurementDefinitions.modelId + '/' + stage.MeasurementDefinitions.modelVersion + '/' + stage.MeasurementDefinitions.field) as id,
            (stage.MeasurementDefinitions.modelId + '/' + stage.MeasurementDefinitions.modelVersion) as model,
            stage.MeasurementDefinitions.field,
            stage.MeasurementDefinitions.kind,
            stage.MeasurementDefinitions.dataType,
            stage.MeasurementDefinitions.name
        FROM stage.MeasurementDefinitions
        INNER JOIN (
            SELECT
                C.id,
                MAX(C.SYS_CHANGE_VERSION) as SYS_CHANGE_VERSION
            FROM CHANGETABLE(CHANGES stage.MeasurementDefinitions, @PreviousChangeTrackingVersion) AS C
            GROUP BY C.id
        ) AS CT
        ON CT.id = stage.MeasurementDefinitions.id
        WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
    ) AS changes (id, model, field, kind, dataType, name)
    ON analytics.MeasurementDefinitions.id = changes.id
    WHEN MATCHED THEN
        UPDATE SET
            analytics.MeasurementDefinitions.dataType = changes.dataType,
            analytics.MeasurementDefinitions.name = changes.name
    WHEN NOT MATCHED THEN
        INSERT (id, model, field, kind, dataType, name)
        VALUES (changes.id, changes.model, changes.field, changes.kind, changes.dataType, changes.name)
    ;
END
GO


CREATE PROCEDURE [dbo].[TransformMeasurements] 
	@PreviousChangeTrackingVersion BIGINT, 
	@CurrentChangeTrackingVersion BIGINT
AS
BEGIN
    INSERT INTO analytics.Measurements (deviceId, model, definition, timestamp, numericValue, stringValue, booleanValue)
	SELECT
        stage.Measurements.deviceId,
        (
            CASE
                WHEN stage.Devices.modelId IS NOT NULL AND stage.Devices.modelVersion IS NOT NULL
                THEN (stage.Devices.modelId + '/' + stage.Devices.modelVersion)
                ELSE NULL
            END
        ) AS model,
        (
            CASE
                WHEN stage.Devices.modelId IS NOT NULL AND stage.Devices.modelVersion IS NOT NULL
                THEN (stage.Devices.modelId + '/' + stage.Devices.modelVersion + '/' + stage.Measurements.field)
                ELSE NULL
            END
        ) AS definition,
        stage.Measurements.timestamp,
        stage.Measurements.numericValue,
        stage.Measurements.stringValue,
        stage.Measurements.booleanValue
	FROM stage.Measurements
	INNER JOIN CHANGETABLE(CHANGES stage.Measurements, @PreviousChangeTrackingVersion) AS CT
	ON CT.id = stage.Measurements.id
    LEFT OUTER JOIN stage.Devices
    ON stage.Devices.deviceId = stage.Measurements.deviceId
	WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
END
GO


CREATE PROCEDURE [dbo].[TransformModels] 
	@PreviousChangeTrackingVersion BIGINT, 
	@CurrentChangeTrackingVersion BIGINT
AS
BEGIN
    MERGE analytics.Models
    USING (
        SELECT
            (stage.Models.modelId + '/' + stage.Models.modelVersion) as id,
            stage.Models.modelId,
            stage.Models.modelVersion,
            stage.Models.name,
            stage.Models.description,
            stage.Models.thumbnail
        FROM stage.Models
        INNER JOIN CHANGETABLE(CHANGES stage.Models, @PreviousChangeTrackingVersion) AS CT
        ON CT.id = stage.Models.id
        WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
    ) AS changes (id, modelId, modelVersion, name, description, thumbnail)
    ON analytics.Models.id = changes.id
    WHEN MATCHED THEN
        UPDATE SET
            analytics.Models.name = changes.name,
            analytics.Models.description = changes.description,
            analytics.Models.thumbnail = changes.thumbnail
    WHEN NOT MATCHED THEN
        INSERT (id, modelId, modelVersion, name, description, thumbnail)
        VALUES (changes.id, changes.modelId, changes.modelVersion, changes.name, changes.description, changes.thumbnail)
    ;
END
GO


CREATE PROCEDURE [dbo].[TransformProperties] 
	@PreviousChangeTrackingVersion BIGINT, 
	@CurrentChangeTrackingVersion BIGINT
AS
BEGIN
    MERGE analytics.Properties
    USING (
        SELECT
            (stage.Properties.deviceId + '/' + stage.Properties.kind + '/' + stage.Properties.field) as id,
            stage.Properties.deviceId,
            (
                CASE
                    WHEN stage.Devices.modelId IS NOT NULL AND stage.Devices.modelVersion IS NOT NULL
                    THEN (stage.Devices.modelId + '/' + stage.Devices.modelId)
                    ELSE NULL
                END
            ) AS model,
            (
                CASE
                    WHEN stage.Devices.modelId IS NOT NULL AND stage.Devices.modelVersion IS NOT NULL
                    THEN (stage.Devices.modelId + '/' + stage.Devices.modelId + '/' + stage.Properties.field)
                    ELSE NULL
                END
            ) AS definition,
            stage.Properties.lastUpdated,
            stage.Properties.numericValue,
            stage.Properties.stringValue,
            stage.Properties.booleanValue
        FROM stage.Properties
        INNER JOIN CHANGETABLE(CHANGES stage.Properties, @PreviousChangeTrackingVersion) AS CT
        ON CT.id = stage.Properties.id
        LEFT OUTER JOIN stage.Devices
        ON stage.Devices.deviceId = stage.Properties.deviceId
        WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
    ) AS changes (id, deviceId, model, definition, lastUpdated, numericValue, stringValue, booleanValue)
    ON analytics.Properties.id = changes.id
    WHEN MATCHED THEN
        UPDATE SET
            analytics.Properties.lastUpdated = changes.lastUpdated,
            analytics.Properties.numericValue = changes.numericValue,
            analytics.Properties.stringValue = changes.stringValue,
            analytics.Properties.booleanValue = changes.booleanValue
    WHEN NOT MATCHED THEN
        INSERT (id, deviceId, model, definition, lastUpdated, numericValue, stringValue, booleanValue)
        VALUES (changes.id, changes.deviceId, changes.model, changes.definition, changes.lastUpdated, changes.numericValue, changes.stringValue, changes.booleanValue)
    ;
END
GO


CREATE PROCEDURE [dbo].[TransformPropertyDefinitions] 
	@PreviousChangeTrackingVersion BIGINT, 
	@CurrentChangeTrackingVersion BIGINT
AS
BEGIN
    MERGE analytics.PropertyDefinitions
    USING (
        SELECT
            (stage.PropertyDefinitions.modelId + '/' + stage.PropertyDefinitions.modelVersion + '/' + stage.PropertyDefinitions.kind + '/' + stage.PropertyDefinitions.field) as id,
            (stage.PropertyDefinitions.modelId + '/' + stage.PropertyDefinitions.modelVersion) as model,
            stage.PropertyDefinitions.field,
            stage.PropertyDefinitions.kind,
            stage.PropertyDefinitions.dataType,
            stage.PropertyDefinitions.name,
            stage.PropertyDefinitions.optional
        FROM stage.PropertyDefinitions
        INNER JOIN CHANGETABLE(CHANGES stage.PropertyDefinitions, @PreviousChangeTrackingVersion) AS CT
        ON CT.id = stage.PropertyDefinitions.id
        WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
    ) AS changes (id, model, field, kind, dataType, name, optional)
    ON analytics.PropertyDefinitions.id = changes.id
    WHEN MATCHED THEN
        UPDATE SET
            analytics.PropertyDefinitions.dataType = changes.dataType,
            analytics.PropertyDefinitions.name = changes.name,
            analytics.PropertyDefinitions.optional = changes.optional
    WHEN NOT MATCHED THEN
        INSERT (id, model, field, kind, dataType, name, optional)
        VALUES (changes.id, changes.model, changes.field, changes.kind, changes.dataType, changes.name, changes.optional)
    ;
END
GO

CREATE PROCEDURE [dbo].[UpdateChangeTrackingVersion] 
	@CurrentTrackingVersion BIGINT
AS

BEGIN
	UPDATE [dbo].[ChangeTracking]
	SET [SYS_CHANGE_VERSION] = @CurrentTrackingVersion
END
GO

CREATE TYPE dbo.MeasurementsTableType AS TABLE
(
	[deviceId] [nvarchar](200) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[numericValue] [decimal](30, 10) NULL,
	[stringValue] [nvarchar](max) NULL,
	[booleanValue] [bit] NULL
)
GO

CREATE PROCEDURE [dbo].[InsertMeasurements]
    @tableType dbo.MeasurementsTableType readonly
AS
BEGIN
    INSERT INTO [stage].[Measurements]
	SELECT * FROM @tableType 
END
GO

