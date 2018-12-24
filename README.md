# Navi Faces
A very old project integrated in my unfinished Navi-Game. The architecture was too complicated
in comparison to the MVC-pattern similar architectures I create now.
Also, the description is currently only available in German.

## Developer-Funktionen
Funktionen, denen der Entwickler eine bestimmte Funktion zugeteilt hat. Diesen Funktionen sind unterschiedliche Rechte zu Teil. Daher müssen sie unterschiedlich gehandhabt werden. Beispielsweise erlaubt die Add-Funktion die Übergabe von Widgets, während in der Define-Funktion lediglich Variablen definiert werden können

### 1. Define: 
Definition von Variablen, die für die Objekte benötigt werden.

#### Beispiele:
```
image = Gui.MenuButton;
shinyImage = Gui.MenuButtonHighlight;
pivot = (50.0, 50.0);
```

Die Deklaration von Variablen sollte nach Konvention in alphabetischer Reihenfolge bzgl. des Variablentyps erfolgen. 
Man könnte die Variablen mit den primitiven Variablentypen gängiger Programmiersprachen vergleichen.

### 2. Create: 
Zum Erstellen von Objekten.

#### Beispiele:
```
imgWindow
```

Dient der Erstellung sichtbarer Oberflächenobjekte (img..., btn...). Variablen können, aber müssen nicht an die Objekte übergeben werden. Variablen werden in alphabetischer Reihenfolge übergeben.
