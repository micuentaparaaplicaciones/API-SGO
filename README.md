# API de Gesti�n de Pedidos

Esta API RESTful permite la gesti�n de usuarios, clientes, proveedores, productos, categor�as, �rdenes y detalles de �rdenes. Desarrollada en C# (.NET 8) y utilizando Entity Framework Core con una base de datos Oracle.

---

## Tabla de Contenido

- [Caracter�sticas principales](#caracter�sticas-principales)
- [Requisitos previos](#requisitos-previos)
- [Instalaci�n y configuraci�n](#instalaci�n-y-configuraci�n)
- [Endpoints principales](#endpoints-principales)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Pruebas unitarias](#pruebas-unitarias)
- [Notas adicionales](#notas-adicionales)

---

## Caracter�sticas principales

- **Autenticaci�n JWT**: Registro y login de usuarios con generaci�n de tokens JWT.
- **Gesti�n de entidades**: CRUD completo para usuarios, clientes, proveedores, productos, categor�as, �rdenes y detalles de �rdenes.
- **Operaciones en lote**: Soporte para agregar, actualizar y eliminar m�ltiples registros en una sola petici�n.
- **Manejo centralizado de errores**: Middleware para respuestas de error consistentes y detalladas.
- **Validaciones robustas**: Validaciones de datos a nivel de modelo y respuestas de error estructuradas.
- **Documentaci�n Swagger**: Integraci�n autom�tica para exploraci�n y prueba de la API.

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (recomendado) o cualquier editor compatible con .NET 8
- Acceso a una base de datos Oracle (y cadena de conexi�n v�lida)
- Los siguientes paquetes NuGet se restauran autom�ticamente, pero aseg�rate de tener acceso a ellos:
  - Microsoft.AspNetCore.Authentication.JwtBearer
  - Microsoft.EntityFrameworkCore
  - Oracle.EntityFrameworkCore
  - Swashbuckle.AspNetCore
  - Newtonsoft.Json
  - xUnit (para pruebas)
  - Microsoft.EntityFrameworkCore.InMemory (para pruebas)
  - coverlet.collector (para cobertura de pruebas)
  - Microsoft.NET.Test.Sdk (para pruebas)
- Permisos para ejecutar migraciones de Entity Framework Core en la base de datos Oracle

> Nota: Todos los proyectos est�n configurados para .NET 8.0. Si tienes versiones diferentes de SDK o paquetes, podr�an surgir advertencias o incompatibilidades.

---

## Instalaci�n y configuraci�n

1. **Clona el repositorio**
    ```sh
    git clone <url-del-repositorio>
    cd <carpeta-del-proyecto>
    ```

2. **Configura la cadena de conexi�n y JWT**

    Edita el archivo `API/appsettings.json` y agrega o ajusta las siguientes secciones:
    ```json
    {
      "ConnectionStrings": {
        "ConexionSGO": "<cadena-de-conexion-oracle>"
      },
      "Jwt": {
        "Key": "<clave-secreta>",
        "Issuer": "<issuer>",
        "Audience": "<audience>"
      }
    }
    ```
    > **Importante:** Reemplaza los valores entre `<...>` por tus datos reales. No compartas ni subas este archivo a repositorios p�blicos.

3. **Restaura los paquetes NuGet**
    ```sh
    dotnet restore
    ```

4. **Ejecuta las migraciones de la base de datos**
    ```sh
    dotnet ef database update --project API
    ```
    > Aseg�rate de tener instalado el paquete `dotnet-ef` globalmente si no lo tienes:
    > ```sh
    > dotnet tool install --global dotnet-ef
    > ```

5. **Ejecuta la API**
    ```sh
    dotnet run --project API
    ```

6. **Accede a la documentaci�n Swagger**
    - Abre tu navegador en: `https://localhost:<puerto>/swagger`
    - El puerto se muestra en la consola al iniciar la API o puedes configurarlo en `API/Properties/launchSettings.json`.

---

## Endpoints principales

### Autenticaci�n (`/api/auth`)
- `POST /api/auth/register` � Registra un nuevo usuario.
- `POST /api/auth/login` � Autentica un usuario y retorna un token JWT.

### Usuarios (`/api/user`)
- `GET /api/user` � Lista todos los usuarios.
- `GET /api/user/{id}` � Obtiene un usuario por ID.
- `POST /api/user` � Agrega un usuario.
- `PUT /api/user/{id}` � Actualiza un usuario.
- `DELETE /api/user/{id}` � Elimina un usuario.
- `POST /api/user/add-multiple` � Agrega m�ltiples usuarios.
- `PUT /api/user/update-multiple` � Actualiza m�ltiples usuarios.
- `DELETE /api/user/remove-multiple` � Elimina m�ltiples usuarios.

### Clientes (`/api/customer`)
- `GET /api/customer` � Lista todos los clientes.
- `GET /api/customer/{id}` � Obtiene un cliente por ID.
- `POST /api/customer` � Agrega un cliente.
- `PUT /api/customer/{id}` � Actualiza un cliente.
- `DELETE /api/customer/{id}` � Elimina un cliente.
- `POST /api/customer/add-multiple` � Agrega m�ltiples clientes.
- `PUT /api/customer/update-multiple` � Actualiza m�ltiples clientes.
- `DELETE /api/customer/remove-multiple` � Elimina m�ltiples clientes.

### Proveedores (`/api/supplier`)
- `GET /api/supplier` � Lista todos los proveedores.
- `GET /api/supplier/{id}` � Obtiene un proveedor por ID.
- `POST /api/supplier` � Agrega un proveedor.
- `PUT /api/supplier/{id}` � Actualiza un proveedor.
- `DELETE /api/supplier/{id}` � Elimina un proveedor.
- `POST /api/supplier/add-multiple` � Agrega m�ltiples proveedores.
- `PUT /api/supplier/update-multiple` � Actualiza m�ltiples proveedores.

### Productos (`/api/product`)
- `GET /api/product` � Lista todos los productos.
- `GET /api/product/{id}` � Obtiene un producto por ID.
- `POST /api/product` � Agrega un producto.
- `PUT /api/product/{id}` � Actualiza un producto.
- `DELETE /api/product/{id}` � Elimina un producto.
- `POST /api/product/add-multiple` � Agrega m�ltiples productos.
- `PUT /api/product/update-multiple` � Actualiza m�ltiples productos.
- `DELETE /api/product/remove-multiple` � Elimina m�ltiples productos.

### Categor�as (`/api/category`)
- `GET /api/category` � Lista todas las categor�as.
- `GET /api/category/{id}` � Obtiene una categor�a por ID.
- `POST /api/category` � Agrega una categor�a.
- `PUT /api/category/{id}` � Actualiza una categor�a.
- `DELETE /api/category/{id}` � Elimina una categor�a.
- `POST /api/category/add-multiple` � Agrega m�ltiples categor�as.
- `PUT /api/category/update-multiple` � Actualiza m�ltiples categor�as.
- `DELETE /api/category/remove-multiple` � Elimina m�ltiples categor�as.

### �rdenes (`/api/order`)
- `GET /api/order` � Lista todas las �rdenes.
- `GET /api/order/{id}` � Obtiene una orden por ID.
- `POST /api/order` � Agrega una orden.
- `PUT /api/order/{id}` � Actualiza una orden.
- `DELETE /api/order/{id}` � Elimina una orden.
- `POST /api/order/add-multiple` � Agrega m�ltiples �rdenes.
- `PUT /api/order/update-multiple` � Actualiza m�ltiples �rdenes.
- `DELETE /api/order/remove-multiple` � Elimina m�ltiples �rdenes.

### Detalles de �rdenes (`/api/order-detail`)
- `GET /api/order-detail` � Lista todos los detalles de �rdenes.
- `GET /api/order-detail/{orderId}` � Obtiene los detalles de una orden por su ID.
- `GET /api/order-detail/{orderId}/{productId}` � Obtiene un detalle de orden por ID de orden y producto.
- `POST /api/order-detail` � Agrega un detalle de orden.
- `PUT /api/order-detail/{orderId}/{productId}` � Actualiza un detalle de orden.
- `DELETE /api/order-detail/{orderId}/{productId}` � Elimina un detalle de orden.

> Todos los endpoints siguen la convenci�n RESTful y requieren autenticaci�n JWT, salvo los de registro y login.

---

## Estructura del proyecto

La soluci�n est� organizada en los siguientes directorios principales:

- `API/` � Proyecto principal de la API.
  - `Controllers/` � Controladores que exponen los endpoints REST.
  - `Models/` � Modelos de datos y entidades que representan las tablas de la base de datos.
  - `DataServices/` � Servicios de acceso a datos y l�gica de negocio.
  - `DbContexts/` � Contexto de Entity Framework Core para la conexi�n con Oracle.
  - `Exceptions/` � Middleware y clases para el manejo centralizado de errores.
  - `Properties/` � Archivos de configuraci�n del proyecto, como `launchSettings.json`.
  - `appsettings.json` � Configuraci�n de la aplicaci�n (cadenas de conexi�n, JWT, logging, etc.).

- `API.Tests/` � Proyecto de pruebas unitarias.
  - Pruebas para controladores y servicios de datos, utilizando xUnit y EF Core InMemory.

- `README.md` � Documentaci�n principal del proyecto.
- `<soluci�n>.sln` � Archivo de soluci�n de Visual Studio.

> Esta estructura facilita la escalabilidad, el mantenimiento y la separaci�n de responsabilidades en la aplicaci�n.

---

## Pruebas unitarias

El proyecto incluye un conjunto completo de pruebas unitarias en el directorio `API.Tests/`, utilizando **xUnit** y **Microsoft.EntityFrameworkCore.InMemory** para simular la base de datos.

### Ejecuci�n de pruebas

Desde la ra�z del proyecto, ejecuta:
```sh
dotnet test API.Tests
```

Esto compilar� y ejecutar� todas las pruebas, mostrando un resumen de resultados en la consola.

### Cobertura de pruebas

La soluci�n utiliza **coverlet.collector** para medir la cobertura de c�digo. Para obtener un reporte de cobertura, ejecuta:
```sh
dotnet test API.Tests --collect:"XPlat Code Coverage"
```
El reporte se generar� en la carpeta `TestResults/`.

### Componentes cubiertos por las pruebas

- Controladores de autenticaci�n, usuarios, clientes, proveedores, productos, categor�as, �rdenes y detalles de �rdenes.
- Servicios de acceso a datos (DataServices) para todas las entidades principales.
- Validaciones de negocio y manejo de errores.

### Dependencias de pruebas

- [xUnit](https://xunit.net/) � Framework de pruebas unitarias.
- [Microsoft.EntityFrameworkCore.InMemory](https://learn.microsoft.com/en-us/ef/core/testing/in-memory) � Proveedor de base de datos en memoria para pruebas.
- [coverlet.collector](https://github.com/coverlet-coverage/coverlet) � Recolecci�n de cobertura de c�digo.
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk) � Infraestructura de pruebas para .NET.

> Las pruebas pueden ejecutarse desde Visual Studio (Test Explorer) o desde la l�nea de comandos.

---

## Notas adicionales

- El proyecto est� preparado para desarrollo y despliegue en ambientes seguros.
- Cambia la clave JWT y protege el archivo de configuraci�n en producci�n.
- El middleware de excepciones proporciona trazabilidad y mensajes amigables para el cliente.
- Utiliza Swagger para explorar y probar la API de forma interactiva en desarrollo.

---
## Licencia

Este repositorio es solo para consulta y referencia. Todos los derechos reservados � Rudy Antonio Salas V�quez, 2025.  
No est� permitido copiar, modificar, distribuir ni utilizar el c�digo sin autorizaci�n expresa del autor.

---

**Desarrollado con .NET 8 y Visual Studio 2022**