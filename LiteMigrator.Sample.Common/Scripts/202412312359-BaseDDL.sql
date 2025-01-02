﻿-- Copyright Xeno Innovations, Inc 2019
-- Created by: Damian Suess
-- Date: 2018-08-07
-- Rev: 1
--
-- NOTE:
--  All table must include the following sync info
--   SyncGuid VARCHAR(128) NOT NULL,        -- Has UIDX
--   SyncUpdatedDttm] DATETIME NOT NULL,
--   SyncIsDeleted BIT NOT NULL DEFAULT(0)  -- Is marked for deletion (if possible)

CREATE TABLE Sync2 (
  [Id] VARCHAR(36) PRIMARY KEY NOT NULL,
  [TableName] VARCHAR(128) NOT NULL,
  [UpdatedHash] VARCHAR(128) NOT NULL,
  [LastUpdateDttm] DATETIME NOT NULL
);

CREATE UNIQUE INDEX UIDX_Sync2_TableName ON Sync2(TableName);

-- Customer account
CREATE TABLE Account2 (
  [Id] VARCHAR(36) PRIMARY KEY NOT NULL,
  [OrganizationName] VARCHAR(128) NOT NULL,
  [ContactName] VARCHAR(128) NOT NULL,
  [Email] VARCHAR(128) NOT NULL,
  [Address] VARCHAR(128) NOT NULL,
  [City] VARCHAR(128) NOT NULL,
  [State] VARCHAR(128) NOT NULL,
  [PostalCode] VARCHAR(16) NOT NULL,
  [Phone] VARCHAR(128) NOT NULL,
  [RegistrationKey] VARCHAR(128) NULL,  -- License sync key with cloud
  [RegistrationDttm] DATETIME NULL      -- Last synced DTTM
);

CREATE UNIQUE INDEX UIDX_Account2_OrganizationName ON Account2(OrganizationName);

CREATE TABLE PaymentType2 (
  [Id] VARCHAR(36) PRIMARY KEY NOT NULL,
  [Name] VARCHAR(128) NOT NULL,
  [Description] VARCHAR(128) NOT NULL
);

CREATE UNIQUE INDEX UIDX_PaymentType2_Name ON PaymentType2(Name);
