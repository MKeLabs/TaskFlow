Examen — Variantă 2

Instrucțiuni generale
- Lucrări în repo-ul TaskFlow (calea rădăcină a proiectului).
- Rulați testele
- Livrabile: cod corectat/implementat, migrații generate (dacă e cazul), teste trecute

#### 1) Subiect 1 — Fixarea testului care pică (obligatoriu) (10p)

##### Descriere:
Rulați testele și veți observa unul (sau mai multe) test(e) care eșuează în TaskFlow.BLL.UnitTests. Pare ca cineva a stricat sau a sters o bucata de cod esentiala. Testul se asteapta la ceva, dar codul nu face.

##### Ce trebuie să faceți:
  - Implementați functionalitatea conform comportamentului așteptat 
  - Asigurați-vă că unit-testul trece fara modificari pe test in sine

#### 2) Subiect 2 — Refactor mapping repetitiv în servicii (10p)
##### Descriere:
Există blocuri de cod de mapping inline (de exemplu construirea TaskItemTagEntity în ApplyTagsAsync din TaskItemService). Aceste bucăți se repetă și ar trebui centralizate într-o componentă separată pentru reutilizare.

##### Ce trebuie să faceți:
  - Extrageți logica de mapare a TaskItemTagEntity (și alte mappinguri similare) în metode reutilizabile aflate într-o clasă statică de helper 
  - Refactorizați serviciile care conțin mapping inline (ex: TaskItemService.ApplyTagsAsync) pentru a folosi aceste metode comune.
  - Nu modificați comportamentul logic, doar eliminați duplicarea.

#### 3) Subiect 3 — Adăugare entitate Goal (20p)
##### Descriere scurtă:
Adăugați o entitate **Goal** legată de Project (FK ProjectId). Aceasta servește pentru scopuri de planificare a proiectelor. Aceasta va avea structura de mai jos
  - GoalEntity:
    - int Id
    - int ProjectId (FK către ProjectEntity)
    - string Title (required, max length 250)
    - DateTime TargetDate (business rule: trebuie să fie *cel puțin* ziua următoare — adică >= tomorrow)
    - bool IsCompleted

##### Ce trebuie sa faceti:
Trebuie sa implementati de la cap la coada functionalitatea cu:
  - endpoint GET ALL, READ si DELETE, cu status code urile corecte
  - DELETE trebuie restrictionat la Admin only
  - Entitate, migrari, Repository, Service Layer, Validari Business, Dto, Controllere

---
