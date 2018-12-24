# Navi Faces
A very old project integrated in my unfinished Navi-Game. The architecture was too complicated
in comparison to the MVC-pattern similar architectures I create now.
Also, the description is currently only available in German.

## Developer-Funktionen
Funktionen, denen der Entwickler eine bestimmte Funktion zugeteilt hat. Diesen Funktionen sind unterschiedliche Rechte zu Teil. Daher müssen sie unterschiedlich gehandhabt werden. Beispielsweise erlaubt die ``Add``-Funktion die Übergabe von Widgets, während in der ``Define``-Funktion lediglich Variablen definiert werden können.

### 1. Define 
Definition von Variablen, die für die Objekte benötigt werden.

#### Beispiele:
```
Define:
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
Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor);
lblChooseAMap(text, textColor);
```

wobei: <br>
``lbl``: Datentyp
``ChooseAMap``: Objektname

Dient der Erstellung sichtbarer Oberflächenobjekte (``img..., btn...``). Variablen können, aber müssen nicht an die Objekte übergeben werden. Variablen (``color, highlight, sound, ...``) werden in alphabetischer Reihenfolge übergeben.

### 3. Link
Zum Verlinken von Objekten.

#### Beispiele:
```
Link:
btnNew[N]->MapLoading;
btn{Controls, Credits}->#post;
btnExit[E]->#exit;
```

Zuvor erstellte Objekte werden auf andere Surfaces/Menüs verwiesen und Funktionen zugeteilt. Objekte werden mit dem ``->``- Operator verlinkt. Das heißt, die darauffolgende Funktion oder das jeweilige Surface/Menü wird aufgerufen.

### 4. Add 
Zum Hinzufügen der Objekte zum Surface.

