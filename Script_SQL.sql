------------------------------------------------------------
-- Creación de Base de Datos
------------------------------------------------------------
IF DB_ID('TekusServicesDb') IS NULL
BEGIN
    CREATE DATABASE TekusServicesDb;
END
GO

USE TekusServicesDb;
GO

------------------------------------------------------------
-- Eliminación de tablas (si existen) para recrear esquema
------------------------------------------------------------
IF OBJECT_ID('dbo.ProviderCustomFields', 'U') IS NOT NULL DROP TABLE dbo.ProviderCustomFields;
IF OBJECT_ID('dbo.ProviderServices', 'U') IS NOT NULL DROP TABLE dbo.ProviderServices;
IF OBJECT_ID('dbo.Providers', 'U') IS NOT NULL DROP TABLE dbo.Providers;
IF OBJECT_ID('dbo.Services', 'U') IS NOT NULL DROP TABLE dbo.Services;
IF OBJECT_ID('dbo.Countries', 'U') IS NOT NULL DROP TABLE dbo.Countries;
GO

------------------------------------------------------------
-- Tabla Countries
------------------------------------------------------------
CREATE TABLE Countries (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name    NVARCHAR(100) NOT NULL,
    IsoCode NVARCHAR(10)  NOT NULL
);
GO

------------------------------------------------------------
-- Tabla Services
------------------------------------------------------------
CREATE TABLE Services (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name        NVARCHAR(100)  NOT NULL,
    Description NVARCHAR(500)  NULL,
    HourlyRate  DECIMAL(18,2)  NOT NULL
);
GO

------------------------------------------------------------
-- Tabla Providers
------------------------------------------------------------
CREATE TABLE Providers (
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nit         NVARCHAR(50)  NOT NULL,
    Name        NVARCHAR(150) NOT NULL,
    Email       NVARCHAR(200) NULL,
    PhoneNumber NVARCHAR(50)  NULL,
    CountryId   INT           NOT NULL,

    CONSTRAINT FK_Providers_Countries_CountryId
        FOREIGN KEY (CountryId)
        REFERENCES Countries(Id)
        ON DELETE NO ACTION
);
GO

------------------------------------------------------------
-- Tabla intermedia ProviderServices (muchos a muchos)
------------------------------------------------------------
CREATE TABLE ProviderServices (
    ProviderId INT NOT NULL,
    ServiceId  INT NOT NULL,

    CONSTRAINT PK_ProviderServices PRIMARY KEY (ProviderId, ServiceId),

    CONSTRAINT FK_ProviderServices_Providers_ProviderId
        FOREIGN KEY (ProviderId)
        REFERENCES Providers(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_ProviderServices_Services_ServiceId
        FOREIGN KEY (ServiceId)
        REFERENCES Services(Id)
        ON DELETE CASCADE
);
GO

------------------------------------------------------------
-- Tabla ProviderCustomFields
------------------------------------------------------------
CREATE TABLE ProviderCustomFields (
    Id        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProviderId INT              NOT NULL,
    FieldName  NVARCHAR(200)    NOT NULL,
    FieldValue NVARCHAR(1000)   NOT NULL,

    CONSTRAINT FK_ProviderCustomFields_Providers_ProviderId
        FOREIGN KEY (ProviderId)
        REFERENCES Providers(Id)
        ON DELETE CASCADE
);
GO

------------------------------------------------------------
-- Datos iniciales
------------------------------------------------------------

-- Countries (10 registros)
INSERT INTO Countries (Name, IsoCode) VALUES
('Colombia',  'CO'),
('Peru',      'PE'),
('Mexico',    'MX'),
('Chile',     'CL'),
('Argentina', 'AR'),
('Brasil',    'BR'),
('Ecuador',   'EC'),
('Uruguay',   'UY'),
('Paraguay',  'PY'),
('Bolivia',   'BO');
GO

-- Services (10 registros)
INSERT INTO Services (Name, Description, HourlyRate) VALUES
('Descarga espacial de contenidos', 'Servicio avanzado de descarga desde satélites.', 120.00),
('Desaparición forzada de bytes',   'Eliminación segura y certificada de datos.',     150.00),
('Soporte técnico remoto',          'Soporte en TI remoto 7x24.',                     80.00),
('Monitoreo de redes',              'Monitoreo continuo de infraestructura de red.',  95.00),
('Backup en la nube',               'Respaldo de información en la nube.',            70.00),
('Consultoría de seguridad',        'Asesoría en ciberseguridad.',                    200.00),
('Gestión de bases de datos',       'Administración y tuning de bases de datos.',     130.00),
('Desarrollo a la medida',          'Desarrollo de soluciones de software.',          110.00),
('Integración de sistemas',         'Integración de aplicaciones corporativas.',      140.00),
('Analítica de datos',              'Servicios de BI y analítica avanzada.',          160.00);
GO

-- Providers (10 registros)
INSERT INTO Providers (Nit, Name, Email, PhoneNumber, CountryId) VALUES
('900000001-1', 'Importaciones Tekus S.A.',      'contacto@importtekus.com',   '3001111111', 1),
('900000002-2', 'Servicios Globales S.A.S.',     'info@servglobal.com',        '3002222222', 2),
('900000003-3', 'Soluciones Digitales LTDA',     'ventas@soldigital.com',      '3003333333', 3),
('900000004-4', 'Redes y Más S.A.S.',            'soporte@redesymas.com',      '3004444444', 4),
('900000005-5', 'Cloud Experts S.A.',            'info@cloudexperts.com',      '3005555555', 5),
('900000006-6', 'Seguridad Total Ltda',          'contacto@seguridadtotal.com','3006666666', 1),
('900000007-7', 'DB Masters S.A.S.',             'ventas@dbmasters.com',       '3007777777', 2),
('900000008-8', 'SoftDev Studio',                'info@softdevstudio.com',     '3008888888', 3),
('900000009-9', 'Integraciones del Sur S.A.',    'contacto@integrasur.com',    '3009999999', 4),
('900000010-0', 'Data Insights S.A.S.',          'info@datainsights.com',      '3000000000', 5);
GO

-- ProviderServices (relaciones varios por cada proveedor)
-- Proveedor 1: Importaciones Tekus S.A.
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 5);

-- Proveedor 2
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(2, 1),
(2, 3),
(2, 4),
(2, 6);

-- Proveedor 3
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(3, 3),
(3, 5),
(3, 7);

-- Proveedor 4
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(4, 4),
(4, 6),
(4, 8);

-- Proveedor 5
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(5, 5),
(5, 9),
(5, 10);

-- Proveedor 6
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(6, 2),
(6, 6),
(6, 7);

-- Proveedor 7
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(7, 7),
(7, 3),
(7, 9);

-- Proveedor 8
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(8, 8),
(8, 1),
(8, 10);

-- Proveedor 9
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(9, 9),
(9, 4),
(9, 6);

-- Proveedor 10
INSERT INTO ProviderServices (ProviderId, ServiceId) VALUES
(10, 10),
(10, 2),
(10, 5);
GO

-- ProviderCustomFields (al menos 10 registros)
INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue) VALUES
(1, 'Número de contacto en Marte',        '+99 123 456 789'),
(1, 'Cantidad de mascotas en la nómina',  '5'),
(2, 'Tiempo máximo de respuesta',         '4 horas'),
(2, 'Horario de soporte',                 '24/7'),
(3, 'Nivel de servicio (SLA)',            '99.9%'),
(4, 'Certificación ISO',                  'ISO 27001'),
(5, 'Región principal de operación',      'Latinoamérica'),
(6, 'Tipo de clientes',                   'Empresariales'),
(7, 'Soporte en inglés',                  'Sí'),
(8, 'Cantidad de sedes',                  '3'),
(9, 'Plataforma de monitoreo usada',      'Zabbix'),
(10,'Portal de autoservicio',             'https://portal.datainsights.com');
GO