# Sistema de Gestión de Biblioteca - Implementación DDD

## Descripción del Dominio

Este repositorio implementa un **Sistema de Gestión de Biblioteca** siguiendo los principios de **Domain-Driven Design (DDD)**. El objetivo es modelar el núcleo de negocio de una biblioteca, donde se gestionan préstamos y devoluciones de libros, aplicando patrones DDD para expresar las reglas de negocio de manera clara y coherente.

El modelo está diseñado como base académica para comprender cómo estructurar un proyecto DDD en C# .NET 8, manteniendo:

- Entidades y Value Objects con responsabilidades claras
- Invariantes del dominio protegidas
- Eventos de dominio para cambios importantes
- Separación clara entre módulos
- Código limpio y expresivo

## Módulos del Dominio

### 📚 Módulo Libros (`src/Dominio/Libros`)

**Entidad principal: `Libro`**

Representa un libro en la biblioteca. Cada libro tiene:
- `Titulo`: El título del libro (Value Object)
- `Isbn`: Identificador único del libro (Value Object)
- `Autor`: El autor del libro (Value Object)
- `Estado`: Estado actual de disponibilidad (Disponible, Prestado, NoDisponible)
- `CreadoEnUtc`: Fecha de creación
- `UltimoPrestamo`: Fecha del último préstamo

**Responsabilidades del dominio:**
- Validar que un libro no se puede prestar si ya está prestado
- Registrar cuando un libro es prestado
- Registrar cuando un libro es devuelto
- Mantener coherencia del estado de disponibilidad

**Métodos clave:**
- `Libro.Crear()`: Factory method para crear libros
- `libro.PrestarLibro()`: Registra el préstamo de un libro
- `libro.RegistrarDevolucion()`: Registra la devolución de un libro

**Eventos de dominio:**
- `LibroCreadoEventoDominio`: Se dispara cuando se registra un nuevo libro
- `LibroPrestamoRegistradoEventoDominio`: Se dispara cuando se registra un préstamo
- `LibroDevolucionRegistradaEventoDominio`: Se dispara cuando se registra una devolución

### 👥 Módulo Usuarios (`src/Dominio/Usuarios`)

**Entidad principal: `Usuario`**

Representa un usuario de la biblioteca. Cada usuario tiene:
- `Nombre`: Nombre del usuario (Value Object)
- `Apellido`: Apellido del usuario (Value Object)
- `CorreoElectronico`: Email de contacto (Value Object)

**Responsabilidades del dominio:**
- Identificar de manera única a cada usuario
- Registrar información de contacto

**Métodos clave:**
- `Usuario.Crear()`: Factory method para crear usuarios

**Eventos de dominio:**
- `UsuarioCreadoEventoDominio`: Se dispara cuando se registra un nuevo usuario

### 📖 Módulo Préstamos (`src/Dominio/Prestamos`)

**Entidad principal: `Prestamo`**

Representa un préstamo de un libro a un usuario. Cada préstamo relaciona:
- `LibroId`: Referencia al libro prestado
- `UsuarioId`: Referencia al usuario que toma el préstamo
- `Fechas`: Información de cuándo se prestó y devolvió (Value Object)
- `Estado`: Estado actual del préstamo (Activo, Devuelto, Vencido)
- `CreadoEnUtc`: Fecha de creación del préstamo

**Responsabilidades del dominio:**

1. **Registrar préstamo de libros:**
   - Valida que el libro esté disponible antes de crear el préstamo
   - Cambia automáticamente el estado del libro a "Prestado"
   - Registra la fecha de préstamo
   - Emite evento de préstamo registrado

2. **Registrar devolución de libros:**
   - Valida que el préstamo esté activo
   - Cambia automáticamente el estado del libro a "Disponible"
   - Registra la fecha de devolución
   - Emite evento de devolución registrada

**Reglas del negocio modeladas:**
- ✅ Un libro no puede prestarse si ya está prestado o no disponible
- ✅ Todo préstamo registra fecha de préstamo automáticamente
- ✅ Una devolución actualiza automáticamente la disponibilidad del libro
- ✅ Se mantiene coherencia entre Libro, Préstamo y Usuario

**Métodos clave:**
- `Prestamo.Registrar()`: Factory method para crear préstamos
- `prestamo.RegistrarDevolucion()`: Registra la devolución de un préstamo

**Eventos de dominio:**
- `PrestamoCreadoEventoDominio`: Se dispara cuando se registra un préstamo
- `DevolucionRegistradaEventoDominio`: Se dispara cuando se devuelve un libro

## Abstracciones Compartidas (`src/Dominio/Abstracciones`)

### Entidad Base
- `Entidad`: Clase base para todas las entidades del dominio con soporte para eventos de dominio

### Patrón Result Pattern
Implementamos el patrón **Railway-Oriented Programming** mediante:
- `Resultado`: Representa operaciones exitosas o fallidas
- `Resultado<T>`: Genérico para operaciones que devuelven un valor
- Evita excepciones para flujos de error esperados

### Manejo de Errores
- `Error`: Record que encapsula código y nombre del error
- `ErroresLibro`: Catálogo de errores del módulo Libros
- `ErroresPrestamo`: Catálogo de errores del módulo Préstamos
- `ErroresUsuario`: Catálogo de errores del módulo Usuarios

### Eventos de Dominio
- `IEventoDominio`: Interfaz base para todos los eventos de dominio

## Patrones DDD Aplicados

### 1. **Entidades**
Las entidades del dominio (`Libro`, `Prestamo`, `Usuario`) tienen identidad única (Id) y ciclo de vida.

```csharp
public sealed class Libro : Entidad
{
    // Constructor privado - fuerza el uso de factory methods
    private Libro(...) { }
    
    // Factory method - valida reglas del dominio
    public static Resultado<Libro> Crear(Titulo titulo, Isbn isbn, Autor autor, DateTime utcNow)
}
```

### 2. **Value Objects**
Expresan conceptos del negocio de forma segura y reutilizable.

```csharp
public record Titulo(string Valor);
public record Isbn(string Valor);
public record Autor(string Valor);
public record Nombre(string Valor);
public record FechasPrestamo(DateTime FechaPrestamoUtc, DateTime? FechaDevolucionUtc);
```

### 3. **Agregados**
Los agregados (`Libro` y `Prestamo`) encapsulan lógica relacionada y protegen invariantes.

### 4. **Invariantes del Dominio**
Las reglas se modelan directamente en el código:

```csharp
public Resultado PrestarLibro(DateTime utcNow)
{
    if (Estado != EstadoDisponibilidad.Disponible)
    {
        return Resultado.Fallo(ErroresLibro.NoDisponible);
    }
    // ... lógica de préstamo
}
```

### 5. **Eventos de Dominio**
Comunicamos cambios importantes en el dominio:

```csharp
libro.RegistrarEventoDominio(new LibroPrestamoRegistradoEventoDominio(libro.Id));
```

### 6. **Interfaces de Repositorio**
Define contratos sin depender de implementación:

```csharp
public interface IRepositorioLibros
{
    Task<Libro?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Libro?> ObtenerPorIsbnAsync(Isbn isbn, CancellationToken cancellationToken = default);
}
```

## Estructura de Carpetas

```
src/Dominio/
├── Abstracciones/          # Base de todas las entidades
│   ├── Entidad.cs          # Clase base con soporte de eventos
│   ├── Error.cs            # Record para errores
│   ├── Resultado.cs        # Pattern Result (Railway-Oriented)
│   └── IEventoDominio.cs   # Interfaz de eventos
├── Libros/                 # Agregado: Libro
│   ├── Libro.cs            # Entidad principal
│   ├── Titulo.cs           # Value Object
│   ├── Isbn.cs             # Value Object
│   ├── Autor.cs            # Value Object
│   ├── EstadoDisponibilidad.cs  # Enumeración de estados
│   ├── ErroresLibro.cs     # Catálogo de errores
│   ├── IRepositorioLibros.cs    # Contrato del repositorio
│   └── Events/
│       ├── LibroCreadoEventoDominio.cs
│       ├── LibroPrestamoRegistradoEventoDominio.cs
│       └── LibroDevolucionRegistradaEventoDominio.cs
├── Usuarios/               # Agregado: Usuario
│   ├── Usuario.cs          # Entidad principal
│   ├── Nombre.cs           # Value Object
│   ├── Apellido.cs         # Value Object
│   ├── CorreoElectronico.cs # Value Object
│   ├── ErroresUsuario.cs   # Catálogo de errores
│   ├── IRepositorioUsuarios.cs  # Contrato del repositorio
│   └── Events/
│       └── UsuarioCreadoEventoDominio.cs
└── Prestamos/              # Agregado: Préstamo
    ├── Prestamo.cs         # Entidad principal
    ├── FechasPrestamo.cs   # Value Object
    ├── EstadoPrestamo.cs   # Enumeración de estados
    ├── ErroresPrestamo.cs  # Catálogo de errores
    ├── IRepositorioPrestamos.cs  # Contrato del repositorio
    └── Events/
        ├── PrestamoCreadoEventoDominio.cs
        └── DevolucionRegistradaEventoDominio.cs
```

## Procesos Implementados

### Proceso 1: Registrar Préstamo de Libros

**Flujo:**
1. Usuario solicita prestar un libro
2. Sistema valida que el libro esté disponible
3. Si es válido:
   - Crea nuevo `Prestamo`
   - Cambiar estado del `Libro` a "Prestado"
   - Registra evento `PrestamoCreadoEventoDominio`
   - Devuelve préstamo exitoso
4. Si falla:
   - Devuelve error explicativo (ej: libro ya prestado)

**Reglas del negocio protegidas:**
- Un libro en estado "Prestado" no puede ser prestado nuevamente
- Un libro en estado "NoDisponible" no puede ser prestado
- Cada préstamo registra automáticamente la fecha de préstamo

**Métodos involucrados:**
```csharp
Libro.PrestarLibro(DateTime utcNow)           // Cambiar estado del libro
Prestamo.Registrar(Libro libro, Guid usuarioId, DateTime utcNow)  // Crear préstamo
```

### Proceso 2: Registrar Devolución de Libros

**Flujo:**
1. Usuario devuelve un libro
2. Sistema valida que el préstamo esté activo
3. Si es válido:
   - Cambiar estado del `Libro` a "Disponible"
   - Cambiar estado del `Préstamo` a "Devuelto"
   - Registra fecha de devolución
   - Registra evento `DevolucionRegistradaEventoDominio`
   - Devuelve confirmación de devolución
4. Si falla:
   - Devuelve error explicativo (ej: préstamo ya devuelto)

**Reglas del negocio protegidas:**
- Un préstamo no activo no puede ser devuelto
- Cambio de estado del libro es automático
- Se registra automáticamente la fecha de devolución

**Métodos involucrados:**
```csharp
Prestamo.RegistrarDevolucion(Libro libro, DateTime utcNow)   // Registrar devolución
Libro.RegistrarDevolucion(DateTime utcNow)                   // Cambiar estado del libro
```

## Value Objects

| Módulo | Value Object | Propiedades |
|--------|------|------------|
| Libros | `Titulo` | string Valor |
| Libros | `Isbn` | string Valor |
| Libros | `Autor` | string Valor |
| Usuarios | `Nombre` | string Valor |
| Usuarios | `Apellido` | string Valor |
| Usuarios | `CorreoElectronico` | string Valor |
| Préstamos | `FechasPrestamo` | DateTime FechaPrestamoUtc, DateTime? FechaDevolucionUtc |

## Enumeraciones

| Módulo | Enumeración | Valores |
|--------|---|---------|
| Libros | `EstadoDisponibilidad` | Disponible (0), Prestado (1), NoDisponible (2) |
| Préstamos | `EstadoPrestamo` | Activo (0), Devuelto (1), Vencido (2) |

## Catálogo de Errores

### Errores del Módulo Libros
- `Libro.NoEncontrado`: No se encontró el libro
- `Libro.NoDisponible`: El libro no está disponible para préstamo
- `Libro.YaEnPrestamo`: El libro ya está en préstamo

### Errores del Módulo Préstamos
- `Prestamo.NoEncontrado`: No se encontró el préstamo
- `Prestamo.YaDevuelto`: El préstamo ya ha sido devuelto
- `Prestamo.NoActivo`: El préstamo no está activo
- `Prestamo.LibroNoDisponible`: El libro no está disponible

### Errores del Módulo Usuarios
- `Usuario.NoEncontrado`: No se encontró el usuario
- `Usuario.CredencialesInvalidas`: Las credenciales no son válidas

## Cómo Extender el Proyecto

Para agregar nuevas funcionalidades siguiendo los mismos patrones:

1. **Crear un nuevo módulo** en `src/Dominio/MiModulo/`
2. **Definir Value Objects** para conceptos específicos del negocio
3. **Crear Entidades** usando constructores privados y factory methods
4. **Definir Errores** en un `static class ErroresMiModulo`
5. **Crear Eventos** en `src/Dominio/MiModulo/Events/`
6. **Definir Interfaces de Repositorio** sin implementación
7. **Proteger invariantes** del dominio en métodos de las entidades

## Reglas del Dominio Expresadas en Código

| Regla | Implementación | Ubicación |
|-------|----------------|-----------|
| Un libro no puede prestarse si no está disponible | Validación en `Libro.PrestarLibro()` | `src/Dominio/Libros/Libro.cs` |
| Todo préstamo registra fecha | Constructor de `Prestamo` con `FechasPrestamo` | `src/Dominio/Prestamos/Prestamo.cs` |
| Devolución actualiza disponibilidad | Método `Prestamo.RegistrarDevolucion()` | `src/Dominio/Prestamos/Prestamo.cs` |
| Coherencia Libro-Préstamo-Usuario | Métodos coordinados entre entidades | Múltiples archivos |

## Compilación y Estructura

- **Framework**: .NET 8.0
- **Lenguaje**: C# con nullable reference types habilitado
- **Patrones**: DDD, Result Pattern, Value Objects, Domain Events
- **Dependencias**: MediatR.Contracts 2.0.1

Compilar proyecto:
```bash
dotnet build
```

Ejecutar pruebas (cuando estén disponibles):
```bash
dotnet test
```

## Decisiones de Diseño

### 1. Entidades Sealed
Todas las entidades son `sealed` para:
- Prevenir herencia no intencionada
- Mejorar performance (menos virtual calls)
- Forzar composición sobre herencia

### 2. Constructores Privados + Factory Methods
Utilizamos este patrón para:
- Validar reglas del dominio en creación
- Inicializar eventos automáticamente
- Prevenir estados inválidos

### 3. Result Pattern en lugar de Excepciones
Para operaciones de dominio esperadas como fallas:
- No lanzamos excepciones para errores de negocio
- Usamos `Resultado<T>` para comunicar errores
- Las excepciones se reservan para errores inesperados

### 4. Métodos de Dominio vs Servicios de Aplicación
- **Métodos de Dominio**: Lógica pura del negocio (Libro.PrestarLibro)
- **Servicios de Aplicación**: Orquestación (próxima capa)

### 5. Inmutabilidad de Value Objects
Todos los Value Objects son `record` (inmutables) para:
- Seguridad en concurrencia
- Facilitar testing
- Expresar intención (no cambia su valor)


No hay aun implementaciones de infraestructura (base de datos, mensajeria, API, etc.) ni casos de uso de aplicacion.

## 2. Vision de arquitectura

### 2.1 Decisiones de arquitectura aplicadas

1. Modelo centrado en dominio (Domain-first)
La solucion parte por definir primero el negocio y sus reglas, en lugar de empezar por controladores, ORM o UI.

2. Capa de dominio aislada
El proyecto [src/Dominio/Dominio.csproj](src/Dominio/Dominio.csproj#L1) solo tiene dependencias minimas y no depende de infraestructura concreta.

3. Contratos en lugar de implementaciones
Los repositorios y unidad de trabajo se definen como interfaces para que el dominio no conozca detalles de persistencia:

- [src/Dominio/Usuarios/IRepositorioUsuarios.cs](src/Dominio/Usuarios/IRepositorioUsuarios.cs#L1)
- [src/Dominio/Departamentos/IRepositorioDepartamentos.cs](src/Dominio/Departamentos/IRepositorioDepartamentos.cs#L1)
- [src/Dominio/Reservas/IRepositorioReservas.cs](src/Dominio/Reservas/IRepositorioReservas.cs#L1)
- [src/Dominio/Abstracciones/IUnidadDeTrabajo.cs](src/Dominio/Abstracciones/IUnidadDeTrabajo.cs#L1)

4. Reglas dentro del agregado, no fuera
Las transiciones de estado de una reserva se realizan dentro de [src/Dominio/Reservas/Reserva.cs](src/Dominio/Reservas/Reserva.cs#L1), evitando logica de negocio dispersa.

5. Modelo explicito de errores y resultados
En vez de usar excepciones para todo, se usa Resultado + Error para comunicar fallos esperables del negocio:

- [src/Dominio/Abstracciones/Resultado.cs](src/Dominio/Abstracciones/Resultado.cs#L1)
- [src/Dominio/Abstracciones/Error.cs](src/Dominio/Abstracciones/Error.cs#L1)

6. Eventos de dominio para desacoplar reacciones
Las entidades registran eventos de dominio, permitiendo reaccionar a cambios de negocio sin acoplarse a infraestructura:

- [src/Dominio/Abstracciones/Entidad.cs](src/Dominio/Abstracciones/Entidad.cs#L1)
- [src/Dominio/Reservas/Events/ReservaConfirmadaEventoDominio.cs](src/Dominio/Reservas/Events/ReservaConfirmadaEventoDominio.cs#L1)

7. Tipos ricos para lenguaje ubicuo
Se prefieren tipos de dominio (Calificacion, RangoFechas, Dinero, EstadoReserva) en lugar de primitivos sueltos.

### 2.2 Como deberia crecer esta arquitectura

Para el trabajo futuro, una evolucion recomendada:

1. Dominio: reglas e invariantes (ya iniciado).
2. Application: casos de uso, comandos/queries, orquestacion transaccional.
3. Infrastructure: EF Core, repositorios concretos, outbox, integraciones externas.
4. API/Web: endpoints, autenticacion, validaciones de entrada, serializacion.
5. Tests: unitarios del dominio + integracion de persistencia + pruebas de contrato API.

## 3. Conceptos DDD usados en este proyecto

## 3.1 Entidades

Una entidad tiene identidad propia y ciclo de vida. En este proyecto heredan de Entidad y usan Id.

- Base de entidades: [src/Dominio/Abstracciones/Entidad.cs](src/Dominio/Abstracciones/Entidad.cs#L1)
- Ejemplo: [src/Dominio/Reservas/Reserva.cs](src/Dominio/Reservas/Reserva.cs#L1)

## 3.2 Agregados y raiz de agregado

Reserva, Usuario, Departamento y Resena modelan comportamientos de negocio y protegen consistencia desde metodos propios.

- Reserva como agregado con transiciones de estado: [src/Dominio/Reservas/Reserva.cs](src/Dominio/Reservas/Reserva.cs#L1)
- Usuario con creacion controlada: [src/Dominio/Usuarios/Usuario.cs](src/Dominio/Usuarios/Usuario.cs#L1)

## 3.3 Objetos de valor (Value Objects)

Representan conceptos sin identidad, comparables por valor e inmutables por diseño.

Ejemplos:

- Dinero: [src/Dominio/Compartido/Dinero.cs](src/Dominio/Compartido/Dinero.cs#L1)
- Moneda: [src/Dominio/Compartido/Moneda.cs](src/Dominio/Compartido/Moneda.cs#L1)
- Rango de fechas: [src/Dominio/Reservas/RangoFechas.cs](src/Dominio/Reservas/RangoFechas.cs#L1)
- Calificacion: [src/Dominio/Resenas/Calificacion.cs](src/Dominio/Resenas/Calificacion.cs#L1)

## 3.4 Enumeraciones ricas (Smart Enum)

En vez de usar enums primitivos, se usa una abstraccion de enumerador con comportamiento.

- Base: [src/Dominio/Abstracciones/Enumerador.cs](src/Dominio/Abstracciones/Enumerador.cs#L1)
- Estado de reserva: [src/Dominio/Reservas/EstadoReserva.cs](src/Dominio/Reservas/EstadoReserva.cs#L1)
- Comodidades con porcentaje de recargo: [src/Dominio/Departamentos/Comodidad.cs](src/Dominio/Departamentos/Comodidad.cs#L1)

## 3.5 Servicio de dominio

Cuando una regla no pertenece naturalmente a una sola entidad, se extrae a servicio de dominio.

- Calculo de precios: [src/Dominio/Reservas/ServicioPrecios.cs](src/Dominio/Reservas/ServicioPrecios.cs#L1)

## 3.6 Eventos de dominio

Cada cambio relevante del negocio puede emitir eventos para reacciones posteriores.

- Interfaz de evento: [src/Dominio/Abstracciones/IEventoDominio.cs](src/Dominio/Abstracciones/IEventoDominio.cs#L1)
- Evento de reserva creada: [src/Dominio/Reservas/Events/ReservaReservadaEventoDominio.cs](src/Dominio/Reservas/Events/ReservaReservadaEventoDominio.cs#L1)
- Evento de usuario creado: [src/Dominio/Usuarios/Events/UsuarioCreadoEventoDominio.cs](src/Dominio/Usuarios/Events/UsuarioCreadoEventoDominio.cs#L1)
- Evento de resena creada: [src/Dominio/Resenas/Events/ResenaCreadaEventoDominio.cs](src/Dominio/Resenas/Events/ResenaCreadaEventoDominio.cs#L1)

## 3.7 Repositorios y unidad de trabajo

Los repositorios son puertos del dominio para cargar/guardar agregados. La unidad de trabajo coordina persistencia transaccional.

- Repositorios: ver seccion 2.1 punto 3.
- Unidad de trabajo: [src/Dominio/Abstracciones/IUnidadDeTrabajo.cs](src/Dominio/Abstracciones/IUnidadDeTrabajo.cs#L1)

## 3.8 Errores de dominio y Result

Los errores esperables del negocio se expresan explicitamente y se retornan como Resultado.

- Resultado: [src/Dominio/Abstracciones/Resultado.cs](src/Dominio/Abstracciones/Resultado.cs#L1)
- Error de Reservas: [src/Dominio/Reservas/ErroresReserva.cs](src/Dominio/Reservas/ErroresReserva.cs#L1)
- Error de Usuarios: [src/Dominio/Usuarios/ErroresUsuario.cs](src/Dominio/Usuarios/ErroresUsuario.cs#L1)
- Error de Departamentos: [src/Dominio/Departamentos/ErroresDepartamento.cs](src/Dominio/Departamentos/ErroresDepartamento.cs#L1)
- Error de Resenas: [src/Dominio/Resenas/ErroresResena.cs](src/Dominio/Resenas/ErroresResena.cs#L1)

## 4. Flujos de negocio modelados

1. Crear usuario
Se crea el agregado Usuario y se registra evento de dominio UsuarioCreado.

2. Reservar departamento
Se calcula precio con ServicioPrecios, se crea Reserva en estado Reservada y se registra evento ReservaReservada.

3. Transiciones de reserva
Desde Reservada se puede Confirmar o Rechazar.
Desde Confirmada se puede Completar o Cancelar (solo si no inicio el periodo).

4. Crear resena
Solo se permite crear resena cuando la reserva esta Completada.

## 5. Invariantes y reglas importantes

- Rango de fechas invalido: inicio no puede ser posterior a fin.
- Calificacion valida: debe estar entre 1 y 5.
- Dinero solo suma montos con la misma moneda.
- No se puede confirmar/rechazar una reserva fuera del estado Reservada.
- No se puede completar/cancelar una reserva fuera del estado Confirmada.
- No se puede reseñar una reserva no completada.

## 6. Guia para estudiantes: como usar esta base

1. Estudia primero el lenguaje del dominio
Recorre las carpetas de Reservas, Usuarios, Departamentos y Resenas para comprender entidades y objetos de valor.

2. Escribe pruebas unitarias del dominio antes de agregar capas
Casos minimos sugeridos:
- transiciones validas e invalidas de Reserva,
- calculo de precio con y sin comodidades,
- validacion de Calificacion y RangoFechas,
- elegibilidad de Resena.

3. Crea la capa Application
Implementa casos de uso (por ejemplo: ReservarDepartamento, ConfirmarReserva, CrearResena) usando repositorios y unidad de trabajo.

4. Crea la capa Infrastructure
Implementa persistencia con EF Core, mapeos de Value Objects, y despacho de eventos de dominio.

5. Expone API o UI
Agrega endpoints y DTOs sin mover reglas de negocio fuera del dominio.

6. Mantiene el dominio limpio
Si una regla es del negocio, debe vivir en Dominio, no en controlador ni repositorio.

## 7. Decisiones que conviene mantener en trabajos futuros

- Mantener nombres del negocio para reforzar lenguaje ubicuo.
- Mantener metodos de fabrica y constructores privados cuando haya invariantes.
- Mantener errores tipados y Resultado para flujos esperables.
- Mantener eventos de dominio para desacoplar efectos secundarios.
- Evitar exponer setters publicos que rompan invariantes.

## 8. Posibles mejoras academicas (siguientes iteraciones)

- Agregar politicas de cancelacion con penalizacion.
- Agregar disponibilidad y calendario por departamento.
- Agregar promedio de calificaciones por departamento.
- Introducir CQRS basico en Application.
- Implementar patron Outbox para eventos de dominio.
- Agregar pruebas de arquitectura para proteger fronteras entre capas.

## 9. Glosario rapido de C#

Esta seccion resume terminos que aparecen mucho en este proyecto y en codigo C# profesional.

### 9.1 var

- Que es: una forma de declarar una variable dejando que el compilador infiera el tipo.
- Importante: var no significa tipo dinamico. El tipo sigue siendo fuerte y fijo.
- Cuando usarlo:
	- cuando el tipo es obvio por el lado derecho,
	- cuando evita repetir tipos largos.
- Cuando evitarlo:
	- cuando hace el codigo ambiguo para quien recien aprende.

Ejemplo simple:

		var total = 10;          // total es int
		var nombre = "Ana";     // nombre es string

### 9.2 abstract

- Que es: una clase base incompleta que no se puede instanciar directamente.
- Para que sirve: definir una plantilla comun para varios tipos hijos.
- En este proyecto: Comodidad se modela como abstraccion con variantes concretas.

### 9.3 sealed

- Que es: una clase que no puede heredarse.
- Para que sirve: proteger el comportamiento de un tipo y evitar extensiones no deseadas.
- En este proyecto: varias entidades y eventos se declaran sealed para fijar su comportamiento.

### 9.4 record

- Que es: un tipo muy util para objetos de valor.
- Ventaja principal: compara por valor (contenido) en lugar de identidad.
- En este proyecto: se usa en Value Objects como Nombre, Dinero, RangoFechas y Calificacion.

### 9.5 interface

- Que es: un contrato de comportamiento sin implementacion.
- Para que sirve: desacoplar dominio de infraestructura.
- En este proyecto: IRepositorioUsuarios, IRepositorioReservas e IUnidadDeTrabajo son contratos.

### 9.6 static

- Que es: miembro o clase que pertenece al tipo y no a una instancia.
- Uso tipico: metodos de fabrica, utilidades o catalogos de errores.
- En este proyecto: Resultado.Exito, Resultado.Fallo y clases de Errores usan esta idea.

### 9.7 Convenciones de nombres

Estas reglas son clave para leer y escribir codigo mantenible.

1. PascalCase
Se escribe cada palabra con inicial mayuscula.

Uso recomendado en C#:
- Clases, records, interfaces, metodos, propiedades.

Ejemplos:
- Reserva
- Crear
- PrecioTotal
- IRepositorioReservas

2. camelCase
Primera palabra en minuscula y siguientes en mayuscula.

Uso recomendado en C#:
- Variables locales y parametros de metodos.

Ejemplos:
- precioTotal
- usuarioId
- creadoEnUtc

3. _underscore (prefijo con guion bajo)
Se usa normalmente para campos privados.

Uso recomendado en C#:
- campos privados que almacenan estado interno.

Ejemplos:
- _valor
- _eventosDominio

Nota para este repositorio:
- Veras muchos nombres en castellano para reforzar lenguaje ubicuo del negocio.
- Lo importante es ser consistente y mantener una sola convencion en todo el proyecto.

### 9.8 Regla practica para decidir nombres

1. Si representa un tipo (clase, record, interfaz): PascalCase.
2. Si es variable local o parametro: camelCase.
3. Si es campo privado: _underscore + camelCase.
4. Si el nombre expresa negocio: priorizar terminos del dominio antes que tecnicismos.

## 10. Comandos utiles

Restaurar y compilar:

- dotnet restore
- dotnet build AirBnb.sln

Formatear codigo (si se usa csharpier):

- dotnet tool restore
- dotnet csharpier format .

---

