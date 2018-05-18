SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
GO

CREATE PROCEDURE [dbo].[TransformMeasurements] 
AS
BEGIN
	DECLARE @PreviousChangeTrackingVersion BIGINT
	DECLARE @CurrentChangeTrackingVersion BIGINT

	SELECT @CurrentChangeTrackingVersion = CHANGE_TRACKING_CURRENT_VERSION()
	SELECT @PreviousChangeTrackingVersion = MAX([SYS_CHANGE_VERSION]) FROM dbo.[ChangeTracking] WHERE TABLE_NAME = 'Measurements' GROUP BY TABLE_NAME;

	BEGIN TRY

		BEGIN TRANSACTION

			INSERT INTO analytics.Measurements (messageId, deviceId, model, definition, timestamp, numericValue, stringValue, booleanValue)
			SELECT
				M.messageId,
				M.deviceId,
				D.model,
				(
					CASE
						WHEN D.model IS NOT NULL
						THEN (D.model + '/' + M.field)
						ELSE M.field
					END
				) AS definition,
				M.timestamp,
				M.numericValue,
				M.stringValue,
				M.booleanValue
			FROM stage.Measurements AS M WITH(NOLOCK)
			INNER JOIN CHANGETABLE(CHANGES stage.Measurements, @PreviousChangeTrackingVersion) AS CT ON CT.id = M.id
			LEFT OUTER JOIN analytics.Devices AS D WITH(NOLOCK) ON D.deviceId = M.deviceId
			WHERE [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion;

			UPDATE ChangeTracking
			SET SYS_CHANGE_VERSION = @CurrentChangeTrackingVersion
			WHERE TABLE_NAME = 'Measurements';
	
		COMMIT TRAN

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN --RollBack in case of Error

		DECLARE @error nvarchar(4000) = ERROR_MESSAGE();
		DECLARE @severity INT = ERROR_SEVERITY();
		RAISERROR(@error, @severity, 1)
	END CATCH

END
GO


-- Device data may come after measurement data. 
-- Update device data into measurement if device data is empty
CREATE PROCEDURE [dbo].[UpdateMeasurements] 
AS
BEGIN
	DECLARE @PreviousChangeTrackingVersion BIGINT
	DECLARE @CurrentChangeTrackingVersion BIGINT

	SELECT @CurrentChangeTrackingVersion = CHANGE_TRACKING_CURRENT_VERSION()
	SELECT @PreviousChangeTrackingVersion = MAX([SYS_CHANGE_VERSION]) FROM dbo.[ChangeTracking] WHERE TABLE_NAME = 'Devices' GROUP BY TABLE_NAME;

	BEGIN TRY

		BEGIN TRANSACTION
			UPDATE [analytics].[Measurements]
			SET [model] = Devices.model,
				[definition] = Devices.model + '/' + analytics.Measurements.[definition]
			FROM 
				(
				SELECT analytics.Devices.* 
				FROM analytics.Devices 
				INNER JOIN CHANGETABLE(CHANGES analytics.Devices, @PreviousChangeTrackingVersion) AS CT ON CT.deviceId = analytics.Devices.deviceId AND [CT].[SYS_CHANGE_VERSION] <= @CurrentChangeTrackingVersion
				) AS Devices
			WHERE analytics.Measurements.model IS NULL AND Devices.deviceId = analytics.Measurements.deviceId

			UPDATE ChangeTracking
			SET SYS_CHANGE_VERSION = @CurrentChangeTrackingVersion
			WHERE TABLE_NAME = 'Devices';
	
		COMMIT TRAN

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN --RollBack in case of Error

		DECLARE @error nvarchar(4000) = ERROR_MESSAGE();
		DECLARE @severity INT = ERROR_SEVERITY();
		RAISERROR(@error, @severity, 1)
	END CATCH

END
GO


CREATE PROCEDURE [dbo].[InsertMeasurements]
    @tableType dbo.MeasurementsTableType readonly
AS
BEGIN
    INSERT INTO [stage].[Measurements] ([messageId], [deviceId], [timestamp], [field], [numericValue], [stringValue], [booleanValue])
	SELECT [messageId], [deviceId], [timestamp], [field], [numericValue], [stringValue], [booleanValue] FROM @tableType;
END
GO


CREATE PROCEDURE [dbo].[InsertDevices]
    @tableType dbo.DevicesTableType readonly
AS
BEGIN

	MERGE [analytics].[Devices]
    USING (
		SELECT deviceId, model, name, simulated FROM @tableType 
	) AS changes ON changes.deviceId = [analytics].[Devices].deviceId
	WHEN MATCHED THEN
		UPDATE SET
			[analytics].[Devices].model = changes.model,
			[analytics].[Devices].[name] = changes.[name],
			[analytics].[Devices].simulated = changes.simulated
	WHEN NOT MATCHED THEN
		INSERT (deviceId, model, [name], simulated)
		VALUES(changes.deviceId, changes.model, changes.[name], changes.simulated);

END
GO


CREATE PROCEDURE [dbo].[InsertProperties]
    @tableType dbo.PropertiesTableType readonly
AS
BEGIN

	MERGE [analytics].[Properties]
    USING (
		SELECT id, deviceId, model, definition, lastUpdated, numericValue, stringValue, booleanValue FROM @tableType 
	) AS changes ON changes.id = [analytics].[Properties].id
	WHEN MATCHED THEN
		UPDATE SET
			[analytics].[Properties].deviceId = changes.deviceId,
			[analytics].[Properties].model = changes.model,
			[analytics].[Properties].definition = changes.definition,
			[analytics].[Properties].lastUpdated = changes.lastUpdated,
			[analytics].[Properties].numericValue = changes.numericValue,
			[analytics].[Properties].stringValue = changes.stringValue,
			[analytics].[Properties].booleanValue = changes.booleanValue
	WHEN NOT MATCHED THEN
		INSERT (id, deviceId, model, definition, lastUpdated, numericValue, stringValue, booleanValue)
		VALUES(changes.id, changes.deviceId, changes.model, changes.definition, changes.lastUpdated, changes.numericValue, changes.stringValue, changes.booleanValue);

END
GO


CREATE PROCEDURE [dbo].[InsertDeviceTemplates]
    @tableType dbo.ModelsTableType readonly
AS
BEGIN

	MERGE [analytics].[Models]
    USING (
		SELECT id, modelId, modelVersion, [name] FROM @tableType 
	) AS changes ON changes.id = [analytics].[Models].id
	WHEN MATCHED THEN
		UPDATE SET
			[analytics].[Models].modelId = changes.modelId,
			[analytics].[Models].modelVersion = changes.modelVersion,
			[analytics].[Models].[name] = changes.[name]
	WHEN NOT MATCHED THEN
		INSERT (id, modelId, modelVersion, [name])
		VALUES(changes.id, changes.modelId, changes.modelVersion, changes.[name]);

END
GO


CREATE PROCEDURE [dbo].[InsertMeasurementDefinitions]
    @tableType dbo.MeasurementDefinitionsTableType readonly
AS
BEGIN

	MERGE [analytics].[MeasurementDefinitions]
    USING (
		SELECT id, model, field, kind, dataType, [name], category FROM @tableType 
	) AS changes ON changes.id = [analytics].[MeasurementDefinitions].id
	WHEN MATCHED THEN
		UPDATE SET
			[analytics].[MeasurementDefinitions].model = changes.model,
			[analytics].[MeasurementDefinitions].field = changes.field,
			[analytics].[MeasurementDefinitions].kind = changes.kind,
			[analytics].[MeasurementDefinitions].dataType = changes.dataType,
			[analytics].[MeasurementDefinitions].[name] = changes.[name],
			[analytics].[MeasurementDefinitions].category = changes.category
	WHEN NOT MATCHED THEN
		INSERT (id, model, field, kind, dataType, [name], category)
		VALUES(changes.id, changes.model, changes.field, changes.kind, changes.dataType, changes.[name], changes.category);

END
GO


CREATE PROCEDURE [dbo].[InsertPropertyDefinitions]
    @tableType dbo.PropertyDefinitionsTableType readonly
AS
BEGIN

	MERGE [analytics].[PropertyDefinitions]
    USING (
		SELECT id, model, field, kind, dataType, [name] FROM @tableType 
	) AS changes ON changes.id = [analytics].[PropertyDefinitions].id
	WHEN MATCHED THEN
		UPDATE SET
			[analytics].[PropertyDefinitions].model = changes.model,
			[analytics].[PropertyDefinitions].field = changes.field,
			[analytics].[PropertyDefinitions].kind = changes.kind,
			[analytics].[PropertyDefinitions].dataType = changes.dataType,
			[analytics].[PropertyDefinitions].[name] = changes.[name]
	WHEN NOT MATCHED THEN
		INSERT (id, model, field, kind, dataType, [name])
		VALUES(changes.id, changes.model, changes.field, changes.kind, changes.dataType, changes.[name]);

END
GO

CREATE PROCEDURE [dbo].[InsertMessages]
    @tableType dbo.MessagesTableType readonly
AS
BEGIN

	INSERT INTO [analytics].[Messages](id, deviceId, [Timestamp], Size)
	SELECT id, deviceId, [Timestamp], Size
	FROM @tableType;

END
GO
