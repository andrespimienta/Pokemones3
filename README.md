# Proyecto Pokemones - Parte I

## Introducción al formato elegido
La organización detrás de nuestro código para esta parte del proyecto
gira alrededor de la idea de que los datos necesarios principales se
encuentren en un archivo txt.
El archivo "Catalogo.txt" contiene toda la información acerca de los
Pokemones que pueden ser elegidos, sus atributos y sus ataques.

Elegir este formato permite una gran flexibilidad, puesto que facilita quitar,
agregar, o modificar los datos de Pokemones nuevos o ya existentes sin
alterar de ninguna manera el funcionamiento del resto de las clases.

## Principios y ventajas aplicadas
Además, intentamos compartimentar las clases lo más posible, de modo que
los métodos y responsabilidades de cada una tengan sentido, acorde con
la información a la que pueden acceder. Por ejemplo, los pokemones y los
ataques son instanciados por métodos de "LeerArchivo", que es la única
clase que cuenta directamente con los datos necesarios para llevar a
cabo esta función (aplicamos "Creator" y "Expert").

También aplicamos "Expert" en algunas situaciones, como a la hora de
decidir que los métodos "UsarPokemon()" y "GuardarPokemon()" deberían
ser responsabilidad de la clase Jugador y no de la propia clase "Pokemon",
puesto que no le corresponde al Pokemon seleccionarse a sí mismo, sino
a la clase que tenga una colección de Pokemones, permitiéndole decidir
cuál utiliza y cuáles deja guardados en su selección.

Como este hay varios otros ejemplos en los que se ve como cada clase se
asegura de cumplir únicamente con lo que tiene sentido que le corresponda,
a la vez que facilitan métodos específicos para determinar bajo qué
circunstancias pueden ser modificados sus atributos, cumpliendo de esta
forma también con "SRP".

## Lógica y funcionamiento
El programa inicia pidiendo los datos para crear dos objetos de la clase Jugador
que corresponderán a los jugadores que se enfrentarán. Luego imprime el catálgo
de Pokemones posibles utilizando, a través de la fachada, un método de LeerArchivo
que se encarga de imprimir los datos de "Catalogo.txt" de manera más procesada,
con más detalle.

Durante la mayor parte del código de Program, veremos que
interactúa principalmente con los métodos de la clase Fachada, y es esta clase
la que se encarga de llamar a los métodos que correspondan del resto de las
clases existentes.

Luego de mostrar el catálogo, por ejemplo, se le pide al jugador que elija
los seis Pokemones de su selección, para lo cual se llama a "Fachada.ElegirPokemon()"
que a su vez interactúa con LeerArchivo para encontrar e instaciar el Pokemon
elegido, luego agregarlo a la lista de Pokemones de la clase Jugador. Es decir,
con un simple método de Fachada, conectamos de una manera u otra con todas
las demás clases. Este tipo de funcionamiento aplica para todas las demás
acciones que se llevarían a cabo en el contexto de una batalla de Pokemones.

Link a diagramas UML: https://drive.google.com/file/d/1Bj022eGMNfHM8U-DYtGgH-5fZGOtxviK/view?usp=sharing