Markdown
**Título:** ADR-002 Implementación de Caché Distribuida para el Listado Público
**Estado:** Propuesto
**Fecha:** 2026-06-17
**Decisores:** Silva Santiago Ramón, Syniuk Marcos Ivan
**Relacionado:** spec_gestion_eventos, issue_latencia_consultas

**Contexto**
* **Qué problema se está resolviendo:** Se anticipa una latencia alta en las consultas a la base de datos debido al crecimiento de la misma. Específicamente, el listado de eventos es público y será accedido masivamente por usuarios no autenticados que aplican filtros por fechas (eventos a futuro o pasados).
* **Qué restricciones aplican (negocio, técnica, legal):** La solución debe integrarse de forma fluida con ASP.NET Core 8, no debe comprometer la base de datos transaccional principal y debe poder desplegarse fácilmente mediante Docker Compose.
* **Qué datos de proyecto sustentan la decisión:** El ratio de lectura/escritura de los eventos publicados es de aproximadamente 100:1. La información de los eventos publicados cambia con poca frecuencia, por lo que recalcular la consulta en PostgreSQL por cada visita pública es ineficiente.

**Decisión**
* **Qué se decide exactamente:** Introducir **Redis** como un componente nuevo para implementar una capa de caché distribuida (Distributed Cache) orientada a almacenar en memoria los resultados de las consultas del listado público de eventos.
* **Alcance (qué cubre y qué no cubre):** Cubre exclusivamente las consultas de solo lectura de acceso público (endpoints GET). No cubre datos transaccionales críticos (ej. proceso de acreditación o inscripción de participantes).

**Alternativas consideradas**
* **Opción A (Caché Distribuida con Redis):**
    * *Pros:* Extremadamente rápido (in-memory), escala horizontalmente de forma independiente a la API, soporta estructuras de datos complejas, permite invalidar la caché fácilmente.
    * *Contras:* Añade un nuevo componente a la infraestructura (mayor complejidad de mantenimiento).
* **Opción B (IMemoryCache - Caché en memoria nativa de .NET):**
    * *Pros:* Cero infraestructura extra, se implementa con un par de líneas de código.
    * *Contras:* Si escalamos la API a múltiples instancias/contenedores, cada uno tendrá su propia caché desincronizada (inconsistencia de datos), consume la memoria RAM del propio contenedor de la API.
* **Opción C (No hacer nada y usar índices de base de datos):**
    * *Pros:* Mantiene la arquitectura simple.
    * *Contras:* No resuelve el cuello de botella del consumo de CPU y conexiones en la base de datos durante picos de tráfico.

**Consecuencias**
* **Beneficios esperados:** Reducción drástica del tiempo de respuesta (latencia) en el listado público, menor carga de CPU en el servidor PostgreSQL.
* **Costos o riesgos que se aceptan:** El dato mostrado al usuario puede estar desactualizado por unos minutos (eventual consistency) hasta que el TTL (Time To Live) de la caché expire. Costo de recursos RAM adicionales para el contenedor de Redis.
* **Impacto en operación y equipo:** Se deberá actualizar el `docker-compose.yml` para incluir la imagen de Redis y modificar la lógica del endpoint GET para que verifique la caché antes de consultar a la BD.

**Plan de implementación**
* **Pasos mínimos para ejecutarla:**
    1. Agregar el contenedor de Redis al entorno de desarrollo (Docker).
    2. Instalar la librería `Microsoft.Extensions.Caching.StackExchangeRedis`.
    3. Configurar el servicio `AddStackExchangeRedisCache` en `Program.cs`.
    4. Implementar el patrón Cache-Aside en el controlador de eventos públicos.

**Dependencias:** Servidor Redis, librería StackExchange.Redis.
**Métrica de éxito:** El endpoint de listado público debe retornar los datos en menos de 50ms bajo carga concurrente (medido en pruebas de integración).

**Triggers de revisión**
* **Qué condiciones obligan a reabrir esta ADR:** Si el costo de mantener el contenedor de Redis supera los beneficios de rendimiento, o si las reglas de negocio exigen sincronización en tiempo real estricta para la vista pública.
* **Fecha sugerida de revisión:** 2026-09-15.

