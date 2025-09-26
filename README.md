# inmobiliatiaNet: Primer entrega

## Funcionalidades incluidas:
- CRUD de Propietarios
- CRUD de Inquilinos
- Filtro de búsqueda por DNI, Nombre o Apellido
- Uso de Bootstrap para diseño y estilos
- Persistencia de datos en MySQL
- Implementación con ADO.NET 

## Base de datos: inmobiliatianet

### Tabla: `propietario`
| Campo     | Tipo        |
|-----------|-------------|
| Id        | int(11)     |
| DNI       | varchar(20) |
| Apellido  | varchar(100)|
| Nombre    | varchar(100)|
| Telefono  | varchar(50) |
| Email     | varchar(100)|

### Tabla: `inquilino`
| Campo     | Tipo        |
|-----------|-------------|
| Id        | int(11)     |
| Nombre    | varchar(50) |
| Apellido  | varchar(50) |
| DNI       | varchar(20) |
| Telefono  | varchar(20) |
| Email     | varchar(100)|

---

## Tecnologías utilizadas:
- ASP.NET Core MVC
- C#
- MySQL
- ADO.NET
- Bootstrap

## Repositorio:
[🔗 GitHub - inmobiliatiaNet](https://github.com/LuciaYuhasz/imnobiliatiaNet)



# inmobiliatiaNet: Segunda entrega

## Funcionalidades incluidas:
CRUD de Inmuebles

CRUD de Contratos

CRUD de Pagos

Vista de detalles para cada entidad

Menú de navegación para acceder a cada sección

Mejora en estilos y presentación general

Diagrama de entidad-relación incluido en el repositorio

Script actualizado de la base de datos

.gitignore configurado para excluir archivos compilados (bin/, obj/)



Base de datos: inmobiliatianet
### Tabla: `inmueble`
| Campo         | Tipo         |
|---------------|--------------|
| Id            | int(11)      |
| Direccion     | varchar(100) |
| Uso           | varchar(100) |
| Tipo          | varchar(100) |
| Ambientes     | int(11)      |
| Latitud       | double       |
| Longitud      | double       |
| PropietarioId | int(11)      |

### Tabla: `contrato`
| Campo       | Tipo           |
|-------------|----------------|
| Id          | int(11)        |
| InmuebleId  | int(11)        |
| InquilinoId | int(11)        |
| FechaInicio | date           |
| FechaFin    | date           |
| Monto       | decimal(18,2)  |
| Activo      | tinyint(1)     |

### Tabla: `pago`
| Campo       | Tipo           |
|-------------|----------------|
| Id          | int(11)        |
| ContratoId  | int(11)        |
| NumeroPago  | int(11)        |
| FechaPago   | date           |
| Concepto    | varchar(255)   |
| Importe     | decimal(18,2)  |
| Anulado     | tinyint(1)     |

### Tabla: `usuario`
| Campo     | Tipo          |
|-----------|---------------|
| Id        | int(11)       |
| Nombre    | varchar(100)  |
| Apellido  | varchar(100)  |
| Rol       | varchar(20)   |
| ClaveHash | varchar(255)  |
| AvatarUrl | varchar(255)  


# inmobiliatiaNet: Entrega final

## Funcionalidades :

CRUD de Propietarios

CRUD de Inquilinos

CRUD de Inmuebles

CRUD de Contratos

CRUD de Pagos

CRUD de Usuarios (gestión por administradores)

Autenticación con email y contraseña

Autorización basada en roles:

Solo administradores pueden eliminar registros y gestionar usuarios

Empleados pueden acceder a funcionalidades restringidas sin privilegios de eliminación

Edición de perfil de usuario (nombre, contraseña y avatar)

Eliminación de avatar (usuario queda sin imagen).

Paginación en listados de entidades

Filtros de busquedas

Validaciones en ABM para evitar inconsistencias (ej: no se puede eliminar inmueble vinculado a contrato)

Vista de detalles para cada entidad

Menú de navegación para acceder a cada sección.

Manejo de sesiones de usuario (rol, id, email)

Repositorios implementados con ADO.NET y MySQL


📘 Descripción del Diagrama de Entidad-Relación actualizada :
  

Tabla: inquilino
| Campo    | Tipo         |
| -------- | ------------ |
| Id       | int(11)      |
| Nombre   | varchar(50)  |
| Apellido | varchar(50)  |
| DNI      | varchar(20)  |
| Telefono | varchar(20)  |
| Email    | varchar(100) |


Tabla: inmueble
| Campo         | Tipo          |
| ------------- | ------------- |
| Id            | int(11)       |
| Direccion     | varchar(255)  |
| Uso           | varchar(100)  |
| Tipo          | varchar(100)  |
| Ambientes     | int(11)       |
| Precio        | decimal(10,2) |
| Disponible    | tinyint(1)    |
| Latitud       | double        |
| Longitud      | double        |
| PropietarioId | int(11)       |



Tabla: propietario
| Campo    | Tipo         |
| -------- | ------------ |
| Id       | int(11)      |
| DNI      | varchar(20)  |
| Apellido | varchar(100) |
| Nombre   | varchar(100) |
| Telefono | varchar(50)  |
| Email    | varchar(100) |


Tabla: contrato
| Campo                      | Tipo          |
| -------------------------- | ------------- |
| Id                         | int(11)       |
| InmuebleId                 | int(11)       |
| InquilinoId                | int(11)       |
| FechaInicio                | date          |
| FechaFin                   | date          |
| Monto                      | decimal(10,2) |
| Activo                     | tinyint(1)    |
| FechaTerminacionAnticipada | date          |
| MontoMulta                 | decimal(10,2) |
| ContratoAnteriorId         | int(11)       |
| UsuarioCreadorId           | int(11)       |
| UsuarioTerminadorId        | int(11)       |
| FechaCreacion              | datetime      |
| FechaModificacion          | datetime      |


Tabla: pago
| Campo              | Tipo          |
| ------------------ | ------------- |
| Id                 | int(11)       |
| ContratoId         | int(11)       |
| NumeroPago         | int(11)       |
| FechaPago          | date          |
| Concepto           | varchar(255)  |
| Importe            | decimal(10,2) |
| Anulado            | tinyint(1)    |
| UsuarioAltaId      | int(11)       |
| UsuarioAnulacionId | int(11)       |
| FechaCreacion      | date          |
| FechaModificacion  | datetime      |
| MotivoAnulacion    | varchar(255)  |


Tabla: usuario
| Campo     | Tipo         |
| --------- | ------------ |
| Id        | int(11)      |
| Email     | varchar(100) |
| Nombre    | varchar(100) |
| Apellido  | varchar(100) |
| Rol       | varchar(20)  |
| ClaveHash | varchar(255) |
| AvatarUrl | varchar(255) |


