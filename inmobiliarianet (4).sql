-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1:3306
-- Tiempo de generación: 26-09-2025 a las 20:12:14
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliarianet`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `Id` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `Activo` tinyint(1) DEFAULT 1,
  `FechaTerminacionAnticipada` date DEFAULT NULL,
  `MontoMulta` decimal(10,2) DEFAULT NULL,
  `ContratoAnteriorId` int(11) DEFAULT NULL,
  `UsuarioCreadorId` int(11) DEFAULT NULL,
  `UsuarioTerminadorId` int(11) DEFAULT NULL,
  `FechaCreacion` datetime DEFAULT current_timestamp(),
  `FechaModificacion` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `InmuebleId`, `InquilinoId`, `FechaInicio`, `FechaFin`, `Monto`, `Activo`, `FechaTerminacionAnticipada`, `MontoMulta`, `ContratoAnteriorId`, `UsuarioCreadorId`, `UsuarioTerminadorId`, `FechaCreacion`, `FechaModificacion`) VALUES
(44, 9, 1, '2025-01-01', '2026-01-01', 150000.00, 1, NULL, NULL, NULL, 3, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(45, 19, 2, '2024-03-01', '2025-03-01', 120000.00, 0, '2025-01-10', 10000.00, NULL, 3, 3, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(46, 23, 3, '2025-05-01', '2026-05-01', 180000.00, 1, NULL, NULL, NULL, 3, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(47, 24, 4, '2024-02-01', '2025-02-01', 135000.00, 0, '2024-11-15', 15000.00, NULL, 3, 3, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(48, 11, 5, '2025-06-01', '2026-06-01', 200000.00, 1, NULL, NULL, NULL, 3, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(49, 25, 6, '2025-07-15', '2026-07-15', 220000.00, 1, NULL, NULL, NULL, 5, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(50, 15, 7, '2024-04-01', '2025-04-01', 160000.00, 0, '2025-02-20', 8000.00, NULL, 6, 6, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(51, 26, 8, '2025-08-01', '2026-08-01', 190000.00, 1, '2025-09-26', 380000.00, NULL, 7, 3, '2025-09-25 15:51:57', '2025-09-26 11:04:29'),
(52, 27, 9, '2024-09-01', '2025-09-01', 175000.00, 1, NULL, NULL, NULL, 8, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(53, 29, 10, '2025-09-26', '2026-10-01', 210000.00, 1, NULL, NULL, NULL, 8, NULL, '2025-09-25 15:51:57', '2025-09-26 05:51:40'),
(54, 30, 11, '2024-11-01', '2025-11-01', 155000.00, 0, '2025-08-10', 9500.00, NULL, 7, 7, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(55, 32, 12, '2025-12-01', '2026-12-01', 600000.00, 1, NULL, NULL, NULL, 6, NULL, '2025-09-25 15:51:57', '2025-09-26 05:48:42'),
(56, 33, 13, '2025-01-15', '2026-01-15', 140000.00, 1, NULL, NULL, NULL, 5, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(57, 34, 14, '2024-07-01', '2025-07-01', 185000.00, 1, NULL, NULL, NULL, 5, NULL, '2025-09-25 15:51:57', '2025-09-25 15:51:57'),
(58, 19, 11, '2025-12-01', '2026-12-31', 500000.00, 1, '2026-01-26', 1000000.00, NULL, 3, 3, '2025-09-26 12:51:45', '2025-09-26 12:59:56'),
(59, 19, 11, '2027-04-01', '2027-05-26', 33333.00, 1, NULL, NULL, NULL, 3, NULL, '2025-09-26 12:57:15', '2025-09-26 12:57:15'),
(60, 19, 11, '2025-09-26', '2025-10-03', 333.00, 1, '2025-09-27', 666.00, NULL, 5, 5, '2025-09-26 13:06:45', '2025-09-26 13:08:32');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Uso` varchar(100) NOT NULL,
  `Tipo` varchar(100) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1,
  `Latitud` double DEFAULT NULL,
  `Longitud` double DEFAULT NULL,
  `PropietarioId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Uso`, `Tipo`, `Ambientes`, `Precio`, `Disponible`, `Latitud`, `Longitud`, `PropietarioId`) VALUES
(9, 'Calle Tcuman 123', 'Residencial', 'Departamento', 3, 95000.00, 1, -34.6083, -58.3712, 7),
(11, 'Ruta 8 Km 45', 'Residencial', 'Casa', 5, 250000.00, 1, -34.798, -58.5211, 8),
(14, 'Falucho 1120', 'Residencial', 'Casa', 5, 500000.00, 1, 100000, 2000000, 7),
(15, 'Catamarca 200', 'Residencial', 'Casa', 3, 450000.00, 1, 455555, 566666, 7),
(17, 'Contitucion y Ayacucho', 'Residencial', 'Casa', 3, 600000.00, 1, 2222, 33333, 8),
(18, 'Caseros 2322', 'Comercial', 'Local', 2, 4500000.00, 1, 998, 88888, 5),
(19, 'Buenos Aires 300', 'Residencial', 'Casa', 3, 4500000.00, 1, 4444, 4444, 7),
(20, 'Mitre 123', 'Residencial', 'Departamento', 2, 120000.00, 1, -34.6037, -58.3816, 5),
(21, 'San Martin 456', 'Comercial', 'Local', 1, 350000.00, 1, -34.61, -58.3772, 6),
(23, 'Lavalle 234', 'Residencial', 'Departamento', 3, 250000.00, 1, -34.604, -58.3701, 8),
(24, 'Independencia 345', 'Comercial', 'Oficina', 2, 400000.00, 1, -34.6081, -58.375, 9),
(25, 'Maipú 678', 'Residencial', 'Casa', 5, 650000.00, 1, -34.6003, -58.3802, 10),
(26, 'Sarmiento 890', 'Residencial', 'Departamento', 2, 230000.00, 1, -34.6055, -58.3778, 11),
(27, 'Italia 1010', 'Residencial', 'Casa', 6, 750000.00, 1, -34.607, -58.3764, 12),
(29, 'Dorrego 321', 'Residencial', 'Departamento', 1, 180000.00, 1, -34.602, -58.379, 14),
(30, 'Urquiza 432', 'Residencial', 'Casa', 4, 470000.00, 1, -34.609, -58.381, 15),
(31, 'Rivadavia 543', 'Residencial', 'Casa', 3, 390000.00, 0, -34.606, -58.378, 16),
(32, 'Entre Rios 654', 'Comercial', 'Oficina', 2, 480000.00, 1, -34.6045, -58.3745, 17),
(33, 'Chacabuco 765', 'Residencial', 'Departamento', 2, 200000.00, 1, -34.603, -58.373, 18),
(34, 'España 876', 'Residencial', 'Casa', 5, 690000.00, 1, -34.6025, -58.3725, 19);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `DNI` varchar(20) NOT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Nombre`, `Apellido`, `DNI`, `Telefono`, `Email`) VALUES
(1, 'Lucas', 'Gonzalez', '32145678', '2664123456', 'lucasg@mail.com'),
(2, 'Martina', 'Rodriguez', '33456789', '2664234567', 'martinar@mail.com'),
(3, 'Bruno', 'Fernandez', '34567890', '2664345678', 'brunofernandez@mail.com'),
(4, 'Sofia', 'Lopez', '35678901', '2664456789', 'sofialopez@mail.com'),
(5, 'Mateo', 'Perez', '36789012', '2664567890', 'mateoperez@mail.com'),
(6, 'Valentina', 'Garcia', '37890123', '2664678901', 'valgarcia@mail.com'),
(7, 'Tomas', 'Sanchez', '38901234', '2664789012', 'tomsanchez@mail.com'),
(8, 'Emma', 'Diaz', '39012345', '2664890123', 'emmadiaz@mail.com'),
(9, 'Benjamin', 'Martinez', '40123456', '2664901234', 'benjmartinez@mail.com'),
(10, 'Isabella', 'Romero', '41234567', '2664012345', 'isabromero@mail.com'),
(11, 'Thiago', 'Alvarez', '42345678', '2664123450', 'thiagoalvarez@mail.com'),
(12, 'Catalina', 'Torres', '43456789', '2664234501', 'catorres@mail.com'),
(13, 'Dylan', 'Ruiz', '44567890', '2664345012', 'dylanruiz@mail.com'),
(14, 'Mariana', 'Flores', '45678901', '2664450123', 'miaflores@mail.com'),
(16, 'Laura', 'Carranza', '35688752', '2664856588', 'carranzal@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Concepto` varchar(255) NOT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `Anulado` tinyint(1) DEFAULT 0,
  `UsuarioAltaId` int(11) DEFAULT NULL,
  `UsuarioAnulacionId` int(11) DEFAULT NULL,
  `FechaCreacion` date DEFAULT curdate(),
  `FechaModificacion` date DEFAULT curdate(),
  `MotivoAnulacion` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id`, `ContratoId`, `NumeroPago`, `FechaPago`, `Concepto`, `Importe`, `Anulado`, `UsuarioAltaId`, `UsuarioAnulacionId`, `FechaCreacion`, `FechaModificacion`, `MotivoAnulacion`) VALUES
(21, 44, 1, '2024-07-01', 'Pago mensual Julio', 150000.00, 0, 3, NULL, '2025-09-25', '2025-09-25', NULL),
(22, 45, 2, '2024-08-01', 'Pago mensual Agosto', 150000.00, 0, 3, NULL, '2025-09-25', '2025-09-25', NULL),
(23, 47, 1, '2025-05-01', 'Primer mes', 180000.00, 0, 3, NULL, '2025-09-25', '2025-09-25', NULL),
(24, 48, 2, '2025-06-01', 'Segundo mes', 180000.00, 0, 3, NULL, '2025-09-25', '2025-09-25', NULL),
(25, 49, 1, '2024-02-01', 'Pago único contrato corto', 135000.00, 0, 5, NULL, '2025-09-25', '2025-09-25', NULL),
(26, 50, 1, '2025-06-01', 'Renta junio', 200000.00, 0, 5, NULL, '2025-09-25', '2025-09-25', NULL),
(27, 51, 1, '2025-07-15', 'Renta Julio', 220000.00, 0, 3, NULL, '2025-09-25', '2025-09-25', NULL),
(28, 52, 1, '2024-04-01', 'Primer pago', 160000.00, 1, 6, 6, '2025-09-25', '2025-09-25', 'Pago erróneo'),
(29, 53, 1, '2025-08-01', 'Alquiler agosto', 190000.00, 1, 7, 5, '2025-09-25', '2025-09-26', 'no valido para este mes '),
(30, 53, 1, '2025-09-26', 'Deposito mas dias de septiembre ', 196333.00, 0, 5, NULL, '2025-09-26', '2025-09-26', NULL),
(31, 58, 1, '2025-09-26', 'mesualidad atrasada', 500000.00, 0, 3, NULL, '2025-09-26', '2025-09-26', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `Id` int(11) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Telefono` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `Dni`, `Apellido`, `Nombre`, `Telefono`, `Email`) VALUES
(5, '335854585', 'Gutierrez', 'Alfredo', '2664856585696', 'alfredogutierrez@gmail.com'),
(6, '55845525', 'Becerra', 'Tamara', '2554525459', 'tamarabe@gmail.com'),
(7, '12345678', 'Fernandez', 'Lucia', '2664123456', 'luciaf@gmail.com'),
(8, '87654321', 'Martinez', 'Carlos', '2664987654', 'cmartinez@gmail.com'),
(9, '368995414', 'Zoto', 'Maria Eugenia', '26647856625', 'mariazoto@gmail.com'),
(10, '45123654', 'Pérez', 'Julián', '2664001122', 'julianperez@gmail.com'),
(11, '32145678', 'Gómez', 'Camila', '2664123456', 'camilagomez@hotmail.com'),
(12, '27896543', 'Rodríguez', 'Esteban', '2664234567', 'estebanr@gmail.com'),
(13, '30987654', 'Sánchez', 'Valeria', '2664345678', 'valesanchez@gmail.com'),
(14, '11223344', 'López', 'Mauro', '2664456789', 'maurolopez@yahoo.com'),
(15, '99887766', 'Ramírez', 'Soledad', '2664567890', 'sol.ramirez@outlook.com'),
(16, '66554433', 'Torres', 'Gabriel', '2664678901', 'gtorres@gmail.com'),
(17, '22334455', 'Flores', 'Julieta', '2664789012', 'juliflores@gmail.com'),
(18, '55667788', 'Castro', 'Martín', '2664890123', 'mcastro@hotmail.com'),
(19, '88990011', 'Morales', 'Elena', '2664901234', 'elena.morales@gmail.com'),
(20, '10111213', 'Reyes', 'Nicolás', '2664012345', 'nreyes@gmail.com'),
(21, '14151617', 'Vega', 'Paula', '2664123457', 'pvega@gmail.com'),
(22, '18192021', 'Herrera', 'Axel', '2664234568', 'axelherrera@outlook.com'),
(23, '22232425', 'Molina', 'Agustina', '2664345679', 'agusmolina@gmail.com'),
(24, '26272829', 'Silva', 'Tobías', '2664456780', 'tsilva@gmail.com'),
(26, '58455698', 'Carrizo', 'Manuel', '2664586522', 'manuelacarrizo@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id` int(11) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Rol` varchar(20) NOT NULL,
  `ClaveHash` varchar(255) NOT NULL,
  `AvatarUrl` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`Id`, `Email`, `Nombre`, `Rol`, `ClaveHash`, `AvatarUrl`) VALUES
(3, 'admin@ejemplo.com', 'Administrador', 'Administrador', '$2a$11$290yQdnnwv3uKksNs5JBnu982Sec1ApIFQtCknEfL.LY9o23lR5ci', '/img/avatars/avatarh2.jpg'),
(5, 'pepa@gmail.com', 'Pepa', 'Empleado', '$2a$11$YmnMw54D7TvBzFEdgBDW7.Ptae7TOSZ620NAvXRtIYl6sC9.NG5Qe', '/img/avatars/avatarm1.jpg'),
(6, 'jorgelinaqueve@gmail.com', 'Jorgelina', 'Administrador', '$2a$11$Fr19P6h9SxuU0B7TXcY.MOmbXyBYv2Wp3fi/61cBNJwK7oSezPG72', NULL),
(7, 'javierdominguez@gmial.com', 'Javier Dominguez', 'Empleado', '$2a$11$r42dJF/EBOH9NRmsKt4/Ku2OfMtxrgXskxBDnWRkJ9mOFB4Tsx6rG', NULL),
(8, 'julianaferrer@gmail.com', 'Juliana Ferrer', 'Empleado', '$2a$11$OUyQtF8TZTtMtQhXdVmkPe9VAH13OT.mivzVQ21PLz1Arn6WEtZ9y', NULL),
(9, 'empleado@gmail.com', 'empleado ', 'Empleado', '$2a$11$LE1FlCDAPUOEyCyMeNXkMOkezBmYi0s3uKX5J6yC32yONj4FqDmDW', '/img/avatars/avatarh3.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `InmuebleId` (`InmuebleId`),
  ADD KEY `InquilinoId` (`InquilinoId`),
  ADD KEY `FK_Contrato_ContratoAnterior` (`ContratoAnteriorId`),
  ADD KEY `FK_Contrato_UsuarioAlta` (`UsuarioCreadorId`),
  ADD KEY `FK_Contrato_UsuarioBaja` (`UsuarioTerminadorId`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `PropietarioId` (`PropietarioId`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ContratoId` (`ContratoId`),
  ADD KEY `FK_Pago_UsuarioAlta` (`UsuarioAltaId`),
  ADD KEY `FK_Pago_UsuarioAnulacion` (`UsuarioAnulacionId`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=61;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `FK_Contrato_ContratoAnterior` FOREIGN KEY (`ContratoAnteriorId`) REFERENCES `contrato` (`Id`),
  ADD CONSTRAINT `FK_Contrato_UsuarioAlta` FOREIGN KEY (`UsuarioCreadorId`) REFERENCES `usuario` (`Id`),
  ADD CONSTRAINT `FK_Contrato_UsuarioBaja` FOREIGN KEY (`UsuarioTerminadorId`) REFERENCES `usuario` (`Id`),
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`Id`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilino` (`Id`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietario` (`Id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `FK_Pago_UsuarioAlta` FOREIGN KEY (`UsuarioAltaId`) REFERENCES `usuario` (`Id`),
  ADD CONSTRAINT `FK_Pago_UsuarioAnulacion` FOREIGN KEY (`UsuarioAnulacionId`) REFERENCES `usuario` (`Id`),
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`ContratoId`) REFERENCES `contrato` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
