
/****** Object:  Table [dbo].[USUARIOS]    Script Date: 12/03/2025 13:48:49 ******/

CREATE TABLE [dbo].[USUARIOS](
	[IDUSUARIO] [int] NOT NULL,
	[USERNAME] [nvarchar](60) NULL,
	[ALIAS] [nvarchar](60) NULL,
	[EMAIL] [nvarchar](150) NULL,
	[SALT] [nvarchar](50) NULL,
	[PASS] [varbinary](max) NULL,
	[AVATAR] [nvarchar](150) NULL,
	[EQUIPO] [nvarchar](150) NULL,
	[PAIS] [varchar](3) NULL,
	[FECHA_UNION] [datetime] NULL,
	[CODEAMISTAD] [nvarchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[IDUSUARIO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[paises]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[paises](
	[codigo] [varchar](3) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_PAISES] PRIMARY KEY CLUSTERED 
(
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[V_USUARIOS_FREE]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   VIEW [dbo].[V_USUARIOS_FREE]
AS
	SELECT IDUSUARIO ,ALIAS,USERNAME,EMAIL, AVATAR, EQUIPO, CODEAMISTAD,
	paises.nombre  as Pais,
	CONVERT(Date,FECHA_UNION) as FECHAUNION
	FROM USUARIOS
	JOIN paises
	ON USUARIOS.PAIS=paises.codigo
GO
/****** Object:  Table [dbo].[AMISTAD]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AMISTAD](
	[IDAMISTAD] [int] NOT NULL,
	[USUARIOID] [int] NOT NULL,
	[AMIGOID] [int] NOT NULL,
	[FECHAAMISTAD] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDAMISTAD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CAMISETA]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CAMISETA](
	[IdCamiseta] [int] NOT NULL,
	[Equipo] [varchar](255) NOT NULL,
	[CodigoPais] [varchar](3) NOT NULL,
	[Year] [int] NOT NULL,
	[Marca] [varchar](100) NOT NULL,
	[Equipacion] [varchar](100) NOT NULL,
	[Posicion] [int] NULL,
	[ImagenCamiseta] [nvarchar](500) NULL,
	[Condicion] [varchar](50) NOT NULL,
	[Dorsal] [int] NULL,
	[Jugador] [varchar](255) NULL,
	[Descripcion] [nvarchar](255) NULL,
	[Es_Activa] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[FechaSubida] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCamiseta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[camiseta_etiqueta]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[camiseta_etiqueta](
	[idEtiqueta] [int] NOT NULL,
	[idCamiseta] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idEtiqueta] ASC,
	[idCamiseta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COMENTARIOS]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COMENTARIOS](
	[IDCOMENTARIO] [int] IDENTITY(1,1) NOT NULL,
	[CAMISETAID] [int] NOT NULL,
	[USUARIOID] [int] NOT NULL,
	[TEXTOCOMENTARIO] [varchar](255) NOT NULL,
	[FECHACOMENTARIO] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDCOMENTARIO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LIKES]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LIKES](
	[LIKEID] [int] NOT NULL,
	[CAMISETAID] [int] NOT NULL,
	[USUARIOID] [int] NOT NULL,
	[FECHALIKE] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LIKEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AMISTAD] ([IDAMISTAD], [USUARIOID], [AMIGOID], [FECHAAMISTAD]) VALUES (1, 3, 4, CAST(N'2025-03-07T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[CAMISETA] ([IdCamiseta], [Equipo], [CodigoPais], [Year], [Marca], [Equipacion], [Posicion], [ImagenCamiseta], [Condicion], [Dorsal], [Jugador], [Descripcion], [Es_Activa], [IdUsuario], [FechaSubida]) VALUES (1, N'Atleti', N'ES', 2020, N'Nike', N'Tercera', NULL, N'atm2.jpg', N'Nueva', 22, N'Marcos Llorente', N'Liga', 1, 3, CAST(N'2025-03-03T23:06:14.223' AS DateTime))
INSERT [dbo].[CAMISETA] ([IdCamiseta], [Equipo], [CodigoPais], [Year], [Marca], [Equipacion], [Posicion], [ImagenCamiseta], [Condicion], [Dorsal], [Jugador], [Descripcion], [Es_Activa], [IdUsuario], [FechaSubida]) VALUES (2, N'Rayo Vallecano', N'ES', 2020, N'Umbro', N'Local', NULL, N'atm.jpg', N'Buena', 7, N'RDT', N'Muy bonita', 1, 4, CAST(N'2025-03-10T19:41:59.763' AS DateTime))
INSERT [dbo].[CAMISETA] ([IdCamiseta], [Equipo], [CodigoPais], [Year], [Marca], [Equipacion], [Posicion], [ImagenCamiseta], [Condicion], [Dorsal], [Jugador], [Descripcion], [Es_Activa], [IdUsuario], [FechaSubida]) VALUES (3, N'Rayo Vallecano', N'ES', 2019, N'Kelme', N'Tercera', NULL, N'211444_1.jpg', N'Usada', NULL, NULL, N'Guapa', 1, 3, CAST(N'2025-03-12T08:58:23.963' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[COMENTARIOS] ON 

INSERT [dbo].[COMENTARIOS] ([IDCOMENTARIO], [CAMISETAID], [USUARIOID], [TEXTOCOMENTARIO], [FECHACOMENTARIO]) VALUES (3, 2, 3, N'Que chula!', CAST(N'2025-03-11T18:26:14.387' AS DateTime))
INSERT [dbo].[COMENTARIOS] ([IDCOMENTARIO], [CAMISETAID], [USUARIOID], [TEXTOCOMENTARIO], [FECHACOMENTARIO]) VALUES (4, 1, 3, N'Holaa', CAST(N'2025-03-11T20:32:02.860' AS DateTime))
SET IDENTITY_INSERT [dbo].[COMENTARIOS] OFF
GO
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AF', N'Afganistán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AL', N'Albania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'DE', N'Alemania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AD', N'Andorra')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AO', N'Angola')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AG', N'Antigua y Barbuda')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SA', N'Arabia Saudita')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'DZ', N'Argelia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AR', N'Argentina')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AM', N'Armenia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AU', N'Australia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AT', N'Austria')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AZ', N'Azerbaiyán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BS', N'Bahamas')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BD', N'Bangladés')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BB', N'Barbados')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BH', N'Baréin')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BE', N'Bélgica')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BZ', N'Belice')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BJ', N'Benín')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BY', N'Bielorrusia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MM', N'Birmania (Myanmar)')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BO', N'Bolivia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BA', N'Bosnia y Herzegovina')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BW', N'Botsuana')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BR', N'Brasil')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BN', N'Brunéi')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BG', N'Bulgaria')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BF', N'Burkina Faso')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BI', N'Burundi')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'BT', N'Bután')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CV', N'Cabo Verde')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KH', N'Camboya')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CM', N'Camerún')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CA', N'Canadá')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'QA', N'Catar')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TD', N'Chad')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CL', N'Chile')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CN', N'China')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CY', N'Chipre')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'VA', N'Ciudad del Vaticano')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CO', N'Colombia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KM', N'Comoras')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CG', N'Congo (Brazzaville)')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CD', N'Congo (Kinshasa)')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KP', N'Corea del Norte')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KR', N'Corea del Sur')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CI', N'Costa de Marfil')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CR', N'Costa Rica')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'HR', N'Croacia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CU', N'Cuba')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'DK', N'Dinamarca')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'DM', N'Dominica')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'EC', N'Ecuador')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'EG', N'Egipto')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SV', N'El Salvador')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'AE', N'Emiratos Árabes Unidos')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ER', N'Eritrea')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SK', N'Eslovaquia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SI', N'Eslovenia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ES', N'España')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'FM', N'Estados Federados de Micronesia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'US', N'Estados Unidos')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'EE', N'Estonia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SZ', N'Esuatini')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ET', N'Etiopía')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PH', N'Filipinas')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'FI', N'Finlandia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'FJ', N'Fiyi')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'FR', N'Francia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GA', N'Gabón')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GM', N'Gambia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GE', N'Georgia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GH', N'Ghana')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GD', N'Granada')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GR', N'Grecia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GT', N'Guatemala')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GN', N'Guinea')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GQ', N'Guinea Ecuatorial')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GW', N'Guinea-Bisáu')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GY', N'Guyana')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'HT', N'Haití')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'HN', N'Honduras')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'HU', N'Hungría')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IN', N'India')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ID', N'Indonesia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IQ', N'Irak')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IR', N'Irán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IE', N'Irlanda')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IS', N'Islandia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MH', N'Islas Marshall')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SB', N'Islas Salomón')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IL', N'Israel')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'IT', N'Italia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'JM', N'Jamaica')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'JP', N'Japón')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'JO', N'Jordania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KZ', N'Kazajistán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KE', N'Kenia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KG', N'Kirguistán')
GO
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KI', N'Kiribati')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'XK', N'Kosovo')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KW', N'Kuwait')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LA', N'Laos')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LS', N'Lesoto')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LV', N'Letonia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LB', N'Líbano')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LR', N'Liberia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LY', N'Libia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LI', N'Liechtenstein')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LT', N'Lituania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LU', N'Luxemburgo')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MK', N'Macedonia del Norte')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MG', N'Madagascar')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MY', N'Malasia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MW', N'Malaui')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MV', N'Maldivas')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ML', N'Malí')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MT', N'Malta')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MA', N'Marruecos')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MU', N'Mauricio')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MR', N'Mauritania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MX', N'México')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MD', N'Moldavia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MC', N'Mónaco')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MN', N'Mongolia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ME', N'Montenegro')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'MZ', N'Mozambique')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NA', N'Namibia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NR', N'Nauru')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NP', N'Nepal')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NI', N'Nicaragua')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NE', N'Níger')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NG', N'Nigeria')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NO', N'Noruega')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NZ', N'Nueva Zelanda')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'OM', N'Omán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'NL', N'Países Bajos')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PK', N'Pakistán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PW', N'Palaos')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PA', N'Panamá')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PG', N'Papúa Nueva Guinea')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PY', N'Paraguay')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PE', N'Perú')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PL', N'Polonia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'PT', N'Portugal')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'GB', N'Reino Unido')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CF', N'República Centroafricana')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CZ', N'República Checa')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'DO', N'República Dominicana')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'RW', N'Ruanda')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'RO', N'Rumania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'RU', N'Rusia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'WS', N'Samoa')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'KN', N'San Cristóbal y Nieves')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SM', N'San Marino')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'VC', N'San Vicente y las Granadinas')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LC', N'Santa Lucía')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ST', N'Santo Tomé y Príncipe')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SN', N'Senegal')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'RS', N'Serbia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SC', N'Seychelles')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SL', N'Sierra Leona')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SG', N'Singapur')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SY', N'Siria')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SO', N'Somalia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'LK', N'Sri Lanka')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ZA', N'Sudáfrica')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SD', N'Sudán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SS', N'Sudán del Sur')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SE', N'Suecia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'CH', N'Suiza')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'SR', N'Surinam')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TH', N'Tailandia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TW', N'Taiwán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TZ', N'Tanzania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TJ', N'Tayikistán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TL', N'Timor Oriental')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TG', N'Togo')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TO', N'Tonga')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TT', N'Trinidad y Tobago')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TN', N'Túnez')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TM', N'Turkmenistán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TR', N'Turquía')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'TV', N'Tuvalu')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'UA', N'Ucrania')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'UG', N'Uganda')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'UY', N'Uruguay')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'UZ', N'Uzbekistán')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'VU', N'Vanuatu')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'VE', N'Venezuela')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'VN', N'Vietnam')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'YE', N'Yemen')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'DJ', N'Yibuti')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ZM', N'Zambia')
INSERT [dbo].[paises] ([codigo], [nombre]) VALUES (N'ZW', N'Zimbabue')
GO
INSERT [dbo].[USUARIOS] ([IDUSUARIO], [USERNAME], [ALIAS], [EMAIL], [SALT], [PASS], [AVATAR], [EQUIPO], [PAIS], [FECHA_UNION], [CODEAMISTAD]) VALUES (3, N'Rodrigo G', N'byrodri', N'rodrigo@gmail.com', N':¥e«$U¹l§æXR×ªlÖµ1.|RÕ¥ÛEÞHá=ì¦§ÕíÊ#¥v~§+', 0xADEF27F96D5B9A96221B1C6D8F9F490AB79AB2317793BE5FB5E4641E54F3E16F49CDA1124B4C66E733731472818D8B990BA061D9311267BC29E81AB87996C6D3, N'filemon.jpg', N'Atlético de Madrid', N'ES', CAST(N'2025-02-27T19:28:13.467' AS DateTime), N'A1B2C3D4')
INSERT [dbo].[USUARIOS] ([IDUSUARIO], [USERNAME], [ALIAS], [EMAIL], [SALT], [PASS], [AVATAR], [EQUIPO], [PAIS], [FECHA_UNION], [CODEAMISTAD]) VALUES (4, N'Alvaro Gomez', N'Gomi', N'alvaro@gmail.com', N'-½Wâ	" 1á7\näÏí¯NÒÍtIÐ}>F7FE^õIKyr', 0xC163DA29C36453894D6A5420A015A03D0927D9D91C5F7394D24B129922AC68C8C6B09B7B97AC56D1B656622921E62A94821A84A8613C8BAD6A8CADB0EB53F37A, N'chad.jpeg', N'Rayo Vallecano', N'PE', CAST(N'2025-03-07T22:34:06.267' AS DateTime), N'FC4A2A07-')
GO
/****** Object:  Index [UQ_Amistad]    Script Date: 12/03/2025 13:48:50 ******/
ALTER TABLE [dbo].[AMISTAD] ADD  CONSTRAINT [UQ_Amistad] UNIQUE NONCLUSTERED 
(
	[USUARIOID] ASC,
	[AMIGOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__LIKES__FDF6FF338F998087]    Script Date: 12/03/2025 13:48:50 ******/
ALTER TABLE [dbo].[LIKES] ADD UNIQUE NONCLUSTERED 
(
	[CAMISETAID] ASC,
	[USUARIOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__paises__40F9A2060F280843]    Script Date: 12/03/2025 13:48:50 ******/
ALTER TABLE [dbo].[paises] ADD UNIQUE NONCLUSTERED 
(
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__paises__72AFBCC61099E7BF]    Script Date: 12/03/2025 13:48:50 ******/
ALTER TABLE [dbo].[paises] ADD UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__USUARIOS__6AE0AB6123301B69]    Script Date: 12/03/2025 13:48:50 ******/
ALTER TABLE [dbo].[USUARIOS] ADD UNIQUE NONCLUSTERED 
(
	[CODEAMISTAD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Usuarios_Email]    Script Date: 12/03/2025 13:48:50 ******/
ALTER TABLE [dbo].[USUARIOS] ADD  CONSTRAINT [UQ_Usuarios_Email] UNIQUE NONCLUSTERED 
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AMISTAD] ADD  DEFAULT (getdate()) FOR [FECHAAMISTAD]
GO
ALTER TABLE [dbo].[CAMISETA] ADD  DEFAULT (getdate()) FOR [FechaSubida]
GO
ALTER TABLE [dbo].[COMENTARIOS] ADD  DEFAULT (getdate()) FOR [FECHACOMENTARIO]
GO
ALTER TABLE [dbo].[LIKES] ADD  DEFAULT (getdate()) FOR [FECHALIKE]
GO
ALTER TABLE [dbo].[USUARIOS] ADD  DEFAULT (getdate()) FOR [FECHA_UNION]
GO
ALTER TABLE [dbo].[AMISTAD]  WITH CHECK ADD  CONSTRAINT [FK_Amistad_Amigo] FOREIGN KEY([AMIGOID])
REFERENCES [dbo].[USUARIOS] ([IDUSUARIO])
GO
ALTER TABLE [dbo].[AMISTAD] CHECK CONSTRAINT [FK_Amistad_Amigo]
GO
ALTER TABLE [dbo].[AMISTAD]  WITH CHECK ADD  CONSTRAINT [FK_Amistad_Usuario] FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIOS] ([IDUSUARIO])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AMISTAD] CHECK CONSTRAINT [FK_Amistad_Usuario]
GO
ALTER TABLE [dbo].[CAMISETA]  WITH CHECK ADD  CONSTRAINT [fk_cam_pais] FOREIGN KEY([CodigoPais])
REFERENCES [dbo].[paises] ([codigo])
GO
ALTER TABLE [dbo].[CAMISETA] CHECK CONSTRAINT [fk_cam_pais]
GO
ALTER TABLE [dbo].[CAMISETA]  WITH CHECK ADD  CONSTRAINT [fk_cam_usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[USUARIOS] ([IDUSUARIO])
GO
ALTER TABLE [dbo].[CAMISETA] CHECK CONSTRAINT [fk_cam_usuario]
GO
ALTER TABLE [dbo].[camiseta_etiqueta]  WITH CHECK ADD  CONSTRAINT [fk_cam_camiseta] FOREIGN KEY([idCamiseta])
REFERENCES [dbo].[camisetas] ([IdCamiseta])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[camiseta_etiqueta] CHECK CONSTRAINT [fk_cam_camiseta]
GO
ALTER TABLE [dbo].[camiseta_etiqueta]  WITH CHECK ADD  CONSTRAINT [fk_cam_etiqueta] FOREIGN KEY([idEtiqueta])
REFERENCES [dbo].[etiqueta] ([idEtiqueta])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[camiseta_etiqueta] CHECK CONSTRAINT [fk_cam_etiqueta]
GO
ALTER TABLE [dbo].[COMENTARIOS]  WITH CHECK ADD  CONSTRAINT [FKCOMMCAMI] FOREIGN KEY([CAMISETAID])
REFERENCES [dbo].[CAMISETA] ([IdCamiseta])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[COMENTARIOS] CHECK CONSTRAINT [FKCOMMCAMI]
GO
ALTER TABLE [dbo].[COMENTARIOS]  WITH CHECK ADD  CONSTRAINT [FKCOMMUSER] FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIOS] ([IDUSUARIO])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[COMENTARIOS] CHECK CONSTRAINT [FKCOMMUSER]
GO
ALTER TABLE [dbo].[LIKES]  WITH CHECK ADD  CONSTRAINT [fk_likes_camiseta] FOREIGN KEY([CAMISETAID])
REFERENCES [dbo].[camisetas] ([IdCamiseta])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LIKES] CHECK CONSTRAINT [fk_likes_camiseta]
GO
ALTER TABLE [dbo].[LIKES]  WITH CHECK ADD  CONSTRAINT [fk_likes_usuario] FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIOS] ([IDUSUARIO])
GO
ALTER TABLE [dbo].[LIKES] CHECK CONSTRAINT [fk_likes_usuario]
GO
ALTER TABLE [dbo].[USUARIOS]  WITH CHECK ADD  CONSTRAINT [FK_USUARIOS_PAIS] FOREIGN KEY([PAIS])
REFERENCES [dbo].[paises] ([codigo])
GO
ALTER TABLE [dbo].[USUARIOS] CHECK CONSTRAINT [FK_USUARIOS_PAIS]
GO
ALTER TABLE [dbo].[AMISTAD]  WITH CHECK ADD  CONSTRAINT [CHK_NoAmistadRepetida] CHECK  (([UsuarioId]<>[AmigoId]))
GO
ALTER TABLE [dbo].[AMISTAD] CHECK CONSTRAINT [CHK_NoAmistadRepetida]
GO
/****** Object:  StoredProcedure [dbo].[SP_OBTENER_AMIGOS_USUARIO]    Script Date: 12/03/2025 13:48:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_OBTENER_AMIGOS_USUARIO]
(@idusuario int)
as
	SELECT u.IDUSUARIO, u.ALIAS, u.USERNAME, u.EMAIL,u.AVATAR,u.EQUIPO,u.CODEAMISTAD,u.Pais,u.FECHAUNION
	
    FROM V_USUARIOS_FREE u
    INNER JOIN AMISTAD a 
	ON u.IDUSUARIO = a.AMIGOID
    WHERE a.USUARIOID = @idusuario
    UNION
    -- Obtener los amigos donde el usuario es quien recibió la amistad
     SELECT u.IDUSUARIO, u.ALIAS, u.USERNAME, u.EMAIL,u.AVATAR,u.EQUIPO,u.CODEAMISTAD,u.Pais,u.FECHAUNION
	
     FROM V_USUARIOS_FREE u
     INNER JOIN AMISTAD a 
	 ON u.IDUSUARIO = a.USUARIOID
     WHERE a.AMIGOID = @idusuario

GO
