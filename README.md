# Supertonic Portable

Supertonic Portable es un motor de síntesis de voz (Text-to-Speech) completamente local para Windows. Está basado en C# y ONNX Runtime, y ha sido modificado para funcionar sin interacción manual: recibe el texto por línea de comandos y lo reproduce automáticamente por los altavoces de tu computadora.

## Características
- **100% Local**: Funciona offline sin necesidad de APIs pagas ni internet.
- **Reproducción Automática**: Ideal para integrarse con asistentes virtuales, scripts y aplicaciones web (Node-WebKit, Electron, etc.). No requiere reproductores externos.
- **Parametrizable**: Puedes configurar los pasos de síntesis (calidad) y la velocidad de la voz.

## Descarga e Instalación (Release)
Debido al gran tamaño de los modelos acústicos (ONNX), el repositorio de código fuente no incluye los modelos listos para usar.
Para instalarlo sin tener que compilarlo:
1. Ve a la sección de **Releases** de este repositorio en GitHub.
2. Descarga el archivo `supertonic-portable-v1.0.zip`.
3. Descomprímelo en cualquier carpeta de tu computadora.
4. Ejecuta `Supertonic.exe` desde la terminal como se explica a continuación.

## Instrucciones de Uso

Abre una terminal (CMD o PowerShell) en la carpeta donde descomprimiste el proyecto y usa la siguiente sintaxis:

```bash
Supertonic.exe [parámetros opcionales] "El texto que quieres que diga"
```

### Argumentos Opcionales
- `--steps <numero>`: Controla el número de pasos de la red neuronal. A menor cantidad (ej: 20), el audio se genera muchísimo más rápido pero con menor calidad. A mayor cantidad (ej: 50 o más), la voz es de alta calidad pero tarda un poco más. **Valor por defecto: 50**.
- `--speed <decimal>`: Ajusta la velocidad de la voz. **Valor por defecto: 1.3**.

### Ejemplos

**Ejemplo Básico (con valores por defecto):**
```bash
Supertonic.exe "Hola, soy una voz artificial local"
```

**Ejemplo Rápido (baja latencia, ideal para asistentes rápidos):**
```bash
Supertonic.exe --steps 20 --speed 1.5 "Tengo una reunión agendada a las 5 de la tarde."
```

## Compilación desde el Código Fuente
Si eres desarrollador y prefieres compilar la aplicación tú mismo:
1. Asegúrate de tener instalado el **SDK de .NET 9.0**.
2. Clona este repositorio: `git clone <url-del-repo>`
3. Compila el código en modo Release:
   ```bash
   dotnet build -c Release
   ```
4. **IMPORTANTE**: Necesitarás los modelos ONNX. Puedes descargarlos desde la Release y ponerlos en la carpeta `bin/Release/net9.0/assets`.
