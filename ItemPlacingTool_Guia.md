# Item Placing Tool - Guia de entrega

Esta guia explica como montar y usar la herramienta de instanciacion de objetos en Unity. La idea es que el editor tenga una biblioteca de prefabs, permita elegir uno como activo, defina un padre dentro de la escena, instancie con click y permita rotar la ultima instancia con drag del raton. Todo queda registrado con Undo.

## 1. Estructura de archivos

Coloca el script en esta ruta:

- `Assets/Editor/ItemPlacerWindow.cs`

Deja la guia en la raiz del proyecto o en una carpeta `Docs` si prefieres ordenar mejor la entrega:

- `ItemPlacingTool_Guia.md`

## 2. Preparar la escena

1. Abre `Assets/Scenes/SampleScene.unity`.
2. Crea un objeto plano o cualquier superficie donde quieras colocar prefabs.
3. Asegurate de que esa superficie tenga un `Collider`, porque la herramienta usa raycast para detectar donde has hecho click.
4. Si quieres que los objetos se agrupen bajo otro nodo, crea un `Empty GameObject` y usalo como padre de instanciacion.

## 3. Preparar los prefabs

1. Crea una carpeta `Assets/Prefabs`.
2. Mete dentro los objetos que realmente quieras colocar en la escena. Para esta practica va bien usar cosas simples como una caja, una silla, una mesa, una piedra, una arbol o un cubo decorativo.
3. Si quieres que se vea completo, prepara entre 3 y 5 prefabs distintos. Asi puedes demostrar que la herramienta sirve para varias piezas y no solo para una.
4. Para convertir un objeto en prefab, arrastralo desde la jerarquia a `Assets/Prefabs` o usa un objeto ya creado en el proyecto.
5. Arrastra cada prefab desde la vista de proyecto a la ventana del tool para construir la lista de disponibles.
6. El boton `Add Selected Prefab` tambien permite añadir el prefab que tengas seleccionado en ese momento.
7. Si en tu escena solo tienes un plano y algunas piezas de prueba, eso tambien vale; la clave es que los objetos que pongas en la lista sean prefabs de proyecto, no objetos sueltos de la escena.

## 4. Abrir la herramienta

1. En Unity, abre el menu `Tools`.
2. Entra en `Item Placing Tool`.
3. Se abrira una ventana con la lista de prefabs y los ajustes de colocacion.

## 5. Configuracion de la ventana

### Biblioteca de prefabs

En la parte superior veras una lista editable. Cada fila representa un prefab disponible.

- Usa `Add Selected Prefab` para incorporar el prefab seleccionado en el proyecto.
- Usa `X` para quitar un prefab de la lista.
- Usa `Clear Missing` si queda algun hueco por una referencia perdida.

### Prefab activo

El campo `Active Prefab` define el objeto que se colocara en escena cuando hagas click.

### Padre de instanciacion

El campo `Spawn Parent` sirve para que todo lo que generes quede dentro de un objeto concreto de la escena.

- Puedes arrastrar un `Transform` manualmente.
- O pulsar `Use Selected Object As Parent` si ya tienes seleccionado el objeto correcto en la jerarquia.

### Ajustes de colocacion

- `Surface Offset`: separa un poco el objeto de la superficie para evitar que quede incrustado.
- `Drag Rotation Speed`: controla la velocidad de giro cuando arrastras el raton.
- `Align To Surface`: orienta el objeto segun la normal de la superficie detectada.

## 6. Uso paso a paso

1. Selecciona el prefab que quieres usar como activo.
2. Si lo necesitas, elige un padre de escena para agrupar las instancias.
3. Ve a la Scene View.
4. Haz click izquierdo sobre la superficie donde quieras colocar el objeto.
5. Si mantienes pulsado y mueves el raton, la ultima instancia rota mientras arrastras.
6. Suelta el boton para terminar la accion.
7. Si te equivocas, pulsa `Ctrl+Z` para deshacer.

## 7. Comportamiento de la herramienta

La herramienta esta pensada para trabajar dentro del editor, no en tiempo de ejecucion.

- La colocacion se hace con `PrefabUtility.InstantiatePrefab`.
- La jerarquia se mantiene limpia si usas un padre de escena.
- La operacion queda registrada con `Undo`, asi que es reversible.
- Si no hay collider en la superficie, la herramienta usa un plano horizontal a nivel `Y = 0` como respaldo.

## 8. Recomendacion para la entrega

Si tienes que presentarlo como practica, explica la logica en este orden:

1. Ventana personalizada en el editor.
2. Lista de prefabs disponibles.
3. Eleccion del prefab activo.
4. Instanciacion sobre la escena con click.
5. Rotacion con drag de la ultima instancia.
6. Undo para mantener un flujo seguro de edicion.

## 9. Resumen corto para exponer

Esta herramienta es una ventana de editor para Unity que permite colocar prefabs en escena de forma rapida. El usuario elige un prefab activo, define un padre de instanciacion y coloca objetos con un click sobre la superficie. Ademas, puede orientar la ultima instancia arrastrando el raton y deshacer cualquier accion con Undo.