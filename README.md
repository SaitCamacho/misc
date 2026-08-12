# 🧰 Toolbox

Repositorio de acceso universal a diversas herramientas y utilidades de propósito general, organizadas por lenguaje de programación. La idea es tener un único punto de entrada para scripts, snippets y utilidades reutilizables en distintos stacks tecnológicos.

## 📌 Descripción

Este repositorio centraliza herramientas y utilidades desarrolladas en diferentes lenguajes, pensadas para reutilizarse en distintos proyectos sin tener que reescribirlas cada vez. Cada lenguaje vive en su propia carpeta, con su propia forma de instalación y ejecución, pero todas comparten una misma filosofía: **simples, independientes y listas para usar**.

## 📂 Estructura del repositorio

```
toolbox/
├── csharp/          # Utilidades y herramientas en C# / .NET
│   └── ...
├── python/          # Utilidades y herramientas en Python
│   └── ...
├── javascript/       # Utilidades y herramientas en JS/TS
│   └── ...
└── README.md
```

Cada carpeta de lenguaje contiene sus propios proyectos/utilidades como subcarpetas independientes, cada una con su propósito específico.

## 🚀 Uso básico

### C# / .NET
```bash
cd csharp/<nombre-utilidad>
dotnet restore
dotnet run
```

### Python
```bash
cd python/<nombre-utilidad>
pip install -r requirements.txt
python main.py
```

### JavaScript / TypeScript
```bash
cd javascript/<nombre-utilidad>
npm install
npm start
```

> Cada utilidad puede tener su propio README con instrucciones específicas de uso, dependencias y ejemplos.

## 🗂️ Herramientas disponibles

| Lenguaje | Utilidad | Descripción |
|----------|----------|-------------|
| C#       | —        | Pendiente de documentar |
| Python   | —        | Pendiente de documentar |
| JS/TS    | —        | Pendiente de documentar |

*(Actualiza esta tabla conforme agregues nuevas herramientas)*

## 🧭 Convenciones

- Cada utilidad va en su propia subcarpeta dentro del lenguaje correspondiente.
- Cada subcarpeta incluye su propio `README.md` con instrucciones puntuales.
- Se evita compartir dependencias entre utilidades; cada una es independiente.

## 📄 Licencia

Especifica aquí la licencia del repositorio (MIT, GPL, etc.).
