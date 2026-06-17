Título: ADR-001 Selección del Motor de Base de Datos Principal
Estado: Aceptado
Fecha: 2026-06-17
Decisores: Silva Santiago Ramón, Syniuk Marcos Ivan
Relacionado: spec_gestion_eventos, project.md

Contexto

    Qué problema se está resolviendo: Se requiere un motor de persistencia relacional robusto para almacenar la información estructurada del "Software de organización y gestión de eventos académicos", incluyendo usuarios, eventos, inscripciones y acreditaciones.  

Qué restricciones aplican (negocio, técnica, legal): El sistema backend está desarrollado en C# con .NET 8 utilizando Entity Framework Core. El motor debe ser compatible con este ORM, preferentemente de código abierto (open-source) para evitar costos de licenciamiento en la etapa inicial del proyecto, y debe soportar transacciones ACID para garantizar la consistencia en las inscripciones con cupos limitados.  

    Qué datos de proyecto sustentan la decisión: La necesidad de manejar relaciones complejas (ej. Usuarios -> Inscripciones -> Eventos) y la integración nativa que EF Core ofrece con múltiples proveedores relacionales.

Decisión

    Qué se decide exactamente: Se adopta PostgreSQL como el motor de base de datos relacional principal para el entorno de desarrollo y producción.

    Alcance (qué cubre y qué no cubre): Cubre el almacenamiento de datos estructurados de la aplicación transaccional. No cubre el almacenamiento de archivos binarios (ej. PDFs generados de los certificados) ni cachés temporales.

Alternativas consideradas

    Opción A (PostgreSQL):

        Pros: Open-source, excelente soporte para JSON (útil para auditorías), alta concurrencia, driver maduro para EF Core (Npgsql).

        Contras: Curva de aprendizaje ligeramente superior en administración avanzada comparado con SQLite o MySQL.

    Opción B (Microsoft SQL Server):

        Pros: Integración perfecta y nativa con el ecosistema .NET, herramientas de gestión visual (SSMS) muy potentes.

        Contras: Costos de licenciamiento en versiones de producción que excedan la versión Express; mayor consumo de recursos en contenedores Docker.

    Opción C (MySQL):

        Pros: Muy popular, amplio soporte en la comunidad, open-source.

        Contras: Cumplimiento del estándar SQL menos estricto que PostgreSQL, gestión de esquemas menos robusta para arquitecturas empresariales complejas.

Consecuencias

    Beneficios esperados: Alta integridad de datos, escalabilidad comprobada, cero costos de licencias y compatibilidad total con nuestro stack de C# en contenedores Docker Alpine.

    Costos o riesgos que se aceptan: Necesidad de configurar volúmenes persistentes en Docker (pgdata) y aprender comandos específicos de administración de PostgreSQL.

    Impacto en operación y equipo: El equipo debe instalar pgAdmin o usar extensiones en Codespaces/VS Code para interactuar con la base de datos visualmente.

Plan de implementación

    Pasos mínimos para ejecutarla:

        Instalar el paquete Npgsql.EntityFrameworkCore.PostgreSQL en el proyecto Web API.

        Configurar la cadena de conexión en appsettings.json.

        Añadir el servicio de PostgreSQL en el archivo docker-compose.yml.

Dependencias: Driver Npgsql.
Métrica de éxito: Capacidad de ejecutar migraciones de Entity Framework y realizar operaciones CRUD sin errores de concurrencia.

Triggers de revisión

    Qué condiciones obligan a reabrir esta ADR: Si el volumen de datos geográficos o no estructurados crece al punto de requerir una base de datos NoSQL especializada.

    Fecha sugerida de revisión: 2026-12-01.
