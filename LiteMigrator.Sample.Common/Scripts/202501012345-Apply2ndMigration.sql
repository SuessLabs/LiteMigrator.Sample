-- Copyright Xeno Innovations, Inc 2019
-- Created by: Damian Suess
-- Date: 2019-03-18
-- Rev: 1
--
-- NOTE:
--  All table must include the following sync info
--   SyncUpdateDttm DATETIME NOT NULL,
--   SyncGuid VARCHAR(128) NOT NULL,
--   SyncIsDeleted BIT NOT NULL DEFAULT(0) -- Is marked for deletion (if possible)

ALTER TABLE [PaymentType2] ADD COLUMN [SyncGuid] VARCHAR(36) NULL;
ALTER TABLE [PaymentType2] ADD COLUMN [SyncUpdatedDttm] DATETIME NULL;
ALTER TABLE [PaymentType2] ADD COLUMN [SyncIsDeleted] BIT NULL DEFAULT(0);

CREATE UNIQUE INDEX IF NOT EXISTS UIDX_PaymentType2_SyncGuid ON PaymentType2(SyncGuid);

INSERT INTO PaymentType2 (Id, Name, Description) VALUES
(0, 'Unknown', 'Unknown'),
(1, 'Cash', 'Cash'),
(2, 'Credit', 'Credit Card'),
(3, 'GiftCard', 'Gift Card');
