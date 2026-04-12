# Konceptuální Definice: Felis Sapiens Simulation Engine

Tento simulační software nevznikl pro pobavení, ale jako tvrdý genetický prediktivní model. Reálný chov je spojený s časem, penězi a neovlivnitelnými proměnnými, a proto je klíčové modelovat a kvantifikovat různé populační zásahy virtuálně.

## Proč simulace existuje?
V rámci projektu se pokoušíme cíleně selektovat vlastnosti, které jinak v přirozené přirozenosti obvykle nevedou ke hmatatelnému darwinovskému přežití – vysoký intelekt vyžaduje spoustu energie, empatie zase sociální toleranci namísto nekompromisní teritoriality. Software nám slouží k modelování dynamiky těchto znaků přes Vektory vlastností (`TraitVector`). 
Simulujeme genetický **Inbreeding** (příbuzenské křížení k zafixování recesivních znaků "géniů") vůči **Outcrossingu** (přilévání cizí krve pro diverzitu a zdraví), což nám následně umožňuje aplikovat různé `IBreedingStrategy` a najít zlatý střed.

## Definice Chovných Strategií

### 1. ElitistBreedingStrategy (Čistý Elitismus)
Veškerá reprodukce je řízena jediným ultimátním samcem s maximálním "IQ" (Fitness kvocientem). I když jsou k dispozici jiné samice, všechna koťata podléhají genomu alfa kočky. To simuluje striktní liniovou inbreedingovou plemenitbu, s drtivým dopadem na fixování špičkových rysů, avšak s rizikem inbrední deprese.
- **Výsledek (ze 100 benchmarků o 7 generacích):** Těsná konvergence, medián dosaženého maxima stoupal prudce na ~156 IQ, ovšem standardní odchylka se smrštila (0.28). Populace ztratila diverzitu. Byly zplozeny kočičí klony s téměř stejnými mentálními rysy. 

### 2. HalfInbreedingBreedingStrategy (Semielitismus, 50% Outcross)
Polovina všech koťat v generaci je plozena absolutně nejchytřejším samcem (alfa modelem), ale pro druhou polovinu se vrh otevírá náhodným samcům ze zbytku populace zvířat, která dosáhla plodnosti a přežila.
- **Výsledek:** Drobný pokles průměrného peaku IQ na ~152 IQ, ovšem **obrovský návrat genetické rozmanitosti (směrodatná odchylka zdraví populace stoupla o 53 % na hodnotu 0.43)** a v maximech se stále (díky zachované polovině linebreeding strategie) tvořili naprostí géniové (95. percentil > 173 IQ). Jedná se tak o jednoznačně stabilnější dlouhodobou cestu chovu.
