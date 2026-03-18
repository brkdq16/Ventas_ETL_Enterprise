🚀 Sales Analysis System - ETL Enterprise

Current Status: 🏗️ Proyecto en Desarrollo Activo.
Este repositorio refleja el progreso incremental del sistema, desde el modelado transaccional hasta la arquitectura analítica final.

---

📁 Estructura del Ecosistema
El proyecto se organiza en 5 módulos estratégicos que separan la documentación, los datos y el código fuente:

- 01_Documentation: Contiene el SRS, el diseño Entidad-Relación (ERD) y evidencias técnicas de la fase de modelado.
- 02_DataSource: Repositorio de archivos CSV, esquemas de JSON y definiciones de API externas.
- 03_Database: Scripts de creación para la base OLTP (3NF) y arquitectura de Data Warehouse.
- 04_SourceCode: Corazón del software organizado bajo Clean Architecture:
- API: Servicios para la exposición de datos procesados.
  - Core: Reglas de negocio, entidades de dominio y lógica de cálculo.
  - Infrastructure: Persistencia en SQL Server y adaptadores para servicios externos.
  - Presentation: Pipeline de consola (Fase inicial) y Worker Service (Motor ETL final).
- 05_Dashboard: Visualización de KPIs y reportes analíticos para la toma de decisiones.

⚡ Especificaciones de Infraestructura (Big Data Ready)
Para garantizar un procesamiento de alto rendimiento (100k registros en < 5 min), se aplicó un tuning de nivel DBA:
💾 Optimización de I/O (Split-Disk)

- Data Files (.mdf): Ubicados en unidad dedicada (D:) para maximizar la velocidad de lectura/escritura.
- Transaction Logs (.ldf): Segregados en la unidad de sistema (C:) para eliminar cuellos de botella por concurrencia.
- Recovery Model: Configurado en Simple para optimizar el espacio en disco durante ingestas masivas.

⚙️ Configuración de Almacenamiento (Storage Tuning)

| Parámetro           | DB OLTP (Transaccional) | DB DW (Analítica) |
| ------------------- | ----------------------- | ----------------- |
| Initial Size (Data) | 1024 MB                 | 1024 MB           |
| Autogrowth (Data)   | 256 MB                  | 256 MB            |
| Initial Size (Log)  | 512 MB                  | 512 MB            |
| Autogrowth (Log)    | 128 MB                  | 128 MB            |

📂 Estrategia de Filegroups
Se implementó una arquitectura de archivos avanzada para la base analítica:

- FG_Staging: Optimizado para la ingesta rápida de datos crudos.
- FG_Analysis: Diseñado para consultas pesadas y generación de KPIs.

🛠️ Tecnologías y Entorno

- Lenguaje: C# (.NET 8)
- Base de Datos: SQL Server 2022/2019 Express Edition.
- Arquitectura: Clean Architecture / Repository Pattern.
- Nota de Portabilidad: Aunque el setup está optimizado para doble unidad física, el sistema es totalmente funcional en configuraciones de disco único.

⚖️ Licencia
Este proyecto se encuentra bajo la Licencia MIT.

---

## Desarrollado por Berkeley Vladimir D.Q. - 2026
