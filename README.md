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




