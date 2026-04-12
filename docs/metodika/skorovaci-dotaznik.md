# Kognitivně-Psychologický Dotazník pro Chovatelskou Praxi: Feline Assessment

Tato metodika tvoří můstek mezi simulačním inženýrstvím (`Felis Sapiens Simulation Engine`) a realitou. Aby software vůbec měl data pro výpočet fitness algoritmu (vlastnostní vektory), musíme chovná zvířata rigorózně a objektivně měřit. Jelikož cílíme na intelekt a "Theory of Mind" (ToM), využíváme transformovanou metodiku z lidské vývojové psychologie a psychometriky.

**ZLATÉ PRAVIDLO HODNOCENÍ (Prevence Clever Hans efektu):**
Vyhodnotitel nesmí svým tělem, tónem hlasu ani očima dávat najevo, jakou reakci od zvířete vyžaduje. Je nutné působit pasivně a odměnu předávat se zpožděním nula vteřin od kýžené reakce, aby nevzniklo přeskočené podmiňování.

---

## 1. Relativní Skórování (Normativní Srovnání v Ranku)
Když hodnotíme chovateli zvíře A relativně, určujeme mu rank v rámci **zvolené referenční množiny $N$ zvířat** (například celosvětová populace dané rasy, nebo vrh 6 koťat). Pro matematický model používáme decily (d1-d10). Relativní test odpovídá na "Je chytřejší než ostatní její sourozenci?".

* **Rozdělení referenční množiny:** Referenční množina by měla být specifikována (např. Vrh C vs Turecká Angora EU).
* **Rankové Dotazy pro Chovatele (Hodnotit na škále P1-P99 proti všem kočkám které znáte):**
  - **Učení se chybou:** "Jak proaktivně jedinec mění svůj postup, když ho předchozí akce dovedla k selhání na rozdíl od klasického instinktivního protlačení vzorce?"
  - **Vokalizační Vzorce:** "Zatímco většina koček vokalizuje jednolitě, jak rozmanitý slovník vrnění/mňoukání využívá tento jedinec v komunikaci s člověkem (intonační křivky)?"

---

## 2. Absolutní Skórování na Feline Big Five a Kognitivních Indexech
Absolutní testy měří schopnost kočky splnit fixně definovanou metriku limitovanou časem či binárním stavem (ZVLÁDL/NEZVLÁDL). Výstup přechází po normalizaci přímo do `TraitVector`.

### A. Fluidní Inteligence (Gf) – Překonávání překážek bez předchozí zkušenosti
Hodnotí se plasticita mozku a neznámé hrozby, tedy jak kočka adaptuje motoriku na nový cíl.
- **Test – Reverzní Překážka (The Bypass):** Hračka (nebo potrava) je umístěna za průhlednou přepážku tvaru U tak, aby ji kočka viděla. Aby ji kočka získala, musí udělat něco instinktivně nepřirozeného – *otočit se zády ke kořisti a obejít přepážku z druhé strany*.
- **Skórování:** 
  1. Zmateně škrábe do plexiskla bez náznaku řešení déle než 1 min (0 bodů).
  2. Náhodou upustí vizuální kontakt a po čase překážku obejde zmatkem (2 body).
  3. Pozoruje rozložení celého prostoru a okamžitě (pod 15 sekund) se obrací ke vstupnímu otvoru vzadu (10 bodů).

### B. Sociální Kognice a "Theory of Mind" (F-ToM) 
Theory of mind je schopnost pochopit, že druhý tvor má své _vlastní_ znalosti a úmysly odlišné od mých. U psů bylo toto dokázáno např. sledováním lidského pohledu.
- **Test – Joint Attention (Sledování Pohledu):** Sedíte naproti zvířeti ve zcela klidné místnosti bez rušivých elementů a pošlete pohled s otočením hlavy výrazně (ale bez trhnutí, spíše v plynulém zajetí zřetele) vlevo do kouta 3 metry od zvířete nahoru.
- **Skórování:**
  1. Kočka to ignoruje, hledí na vaši tvář nebo ruku kvůli vidině pamlsku (0 bodů).
  2. Kočka koukne vaším směrem, ale po chvíli ztratí pozornost a odejde (5 bodů).
  3. Kočka automaticky "převezme" vektor vašeho pohledu, otočí se přímo na prázdný kout a začne kout zkoumat s cílem pochopit, co vás na něm zaujalo, přičemž zkontroluje očima vás i ten kout (10 bodů).

### C. Krystalizovaná Inteligence (Gc) – Práce s pamětí a povely
Sem spadá poslušnost a mapování ustálených fyzikálních pravidel. Jak si zachovávají osvojenou paměť na abstrakce (jména, slova)?
- **Test – Rozlišování Arbitrárních Předmětů:** Dva vizuálně odlišné předměty (např. Kostka vs Koule). Zvíře bylo ve 3 trénincích naučeno (Aktivně stimulováno) dotknout se Koule pro pamlsek. Nyní dáváte povel bez gestikulace po týdenní prodlevě.
- **Skórování:** Absolutní počet správných úderů / interakcí z 10 pokusů. Odolnost na přeskočení pozornosti (např. preferenční kliknutí na pravou stranu místo samotného tvaru).

### D. Feline Temperament (Osobnost) - Feline "Five Factor Model"
Na škále Likertovy stupnice (1-7), určuje tendenci přenášet mutace:
1. **Neuroticismus / Skittishness**: Úroveň leknutí při padajících klíčích. (Vysoké = prchá, Nízké = zkoumá obezřetně a brzy vyhodnotí klíče jako neútočné).
2. **Extraverze / Outgoingness**: Kvílení a nechtěná aktivita, potřeba interakce, explorativita cizích pokojů.
3. **Konkurenceschopnost / Dominance**: Uzurpování jídla proti ostatním zvířatům, vyhánění dospělých jedinců ze spotů (Vysoce dědičné).
4. **Hravost / Spontaneita**: Inkorporování imaginárních her do vlastního chování (lovící fantomy).
5. **Přívětivost (Affectionateness)**: Tolerance na nepříznivou manipulaci majitelem bez okamžité agrese (agrese má být odstupňována signalizací vokální -> ocas -> ošívání -> zasyčení -> nákus. Pokud zvíře přeskakuje fázování a rovnou kouše, má nízkou afektivní inhibici).
