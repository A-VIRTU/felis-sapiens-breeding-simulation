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

### 3. OutcrossBreedingStrategy (Plný Outcross s populací)
Plodné vynikající samice stanice se nepáří se stanicovým Alfa kocourem, ale vždycky je z vnější průměrné populace přiveden náhodný vnější samec. Účelem je totální ochrana před inbrední depresí a maximální genová injekce zdraví do chovu.
- **Výsledek:** Metodika sice zabraňuje jakékoli inbrední depresi a extrémně zvyšuje genetickou divergenci celého chovu, ale pro stanici orientovanou striktně na kognici (Felis Sapiens) znamená devastaci selekčních cílů. Růst elitního IQ se fatálně zpomalí nebo téměř zastaví, jelikož "průměrná" krev z vnějšku neustále strhává supergény zpět do gaussova normálu.

## Základní Linie a Technologické Skórování (AI)

Současná linie chovu je historicky založena na ojedinělém, šťastném setkání s výjimečnou samicí nesoucí kód **Subject Alpha ("Šipka", anglicky *Dart*)**, od níž všechna současná i budoucí koťata fundamentálně odvozujeme. Tato samice byla v kognitivních testech prokazatelně svým chováním nadprůměrná (kdyby se vzalo náhodně šest koček z běžných domácích chovů, tak z nich byla bezpochyby nejlepší).

Tento objev umožnil celý abstraktní problém přesně technologicky uchopit. Naše matematické simulace ukázaly exaktní povahu situace – výběrové elitářské strategie mají hluboká teoretická úskalí (genetic variance vs. inbreeding depression), avšak lze je **v reálném čase as vynaložením realistických nákladů** dovést k jasným a bezpečným výsledkům. 

### Odstranění lidského biasu

Software také demonstruje, že ačkoli je nutné metodologii dále pilovat a vyvíjet technologické způsoby bodování (skórování zvířat), klíčovým imperativem je **zapojení AI, které proces udrží nákladově stabilní a radikálně přesnější**. Vyhodnocování sekvenčních záznamů chování z plně automatizované cesty není na rozdíl od chovatele zatíženo tzv. osobním biasem (náklonností, slepostí, nevědomou úpravou dat). 

Ideálem jsou automatizované kamerové záznamy (`Observation Logs`) zvířete jak v přirozeném felineském prostředí (smečka), tak na ose interakce kočka-člověk. Všechna koťata lze touto algoritmickou analýzou chování ve skupině excelentně setřídit do objektivního pořadí, čímž dospějeme ke stanovení kognitivního skóre, které je přinejmenším stejně exaktní jako tradiční laboratorní technology-based dotazníky, nýbrž nevyžaduje subjektivní ruční lidskou evaluaci.

#Projekt a chov zcela zaštiťuje společnost **A VIRTÙ** (avirtu.net), přičemž plnou koncepční i technickou odpovědnost nese jednatel Viktor Lošťák. 

### Průmyslový standard a "Žádná experimentální teorie"
Zásadně zdůrazňujeme: my sami nevyužíváme ani neprosazujeme žádnou "vlastní převratnou teorii". Striktně se opíráme o obecně uznané exaktní základy genetiky, molekulární a kvantitativní biologie. Naše metodika je postavená na vcelku jednoduchých počítačových simulacích, s nimiž mají naši technologičtí IT experti hluboké zkušenosti z reálného průmyslu.
Ve své podstatě jde o zcela běžnou komplexní optimalizační úlohu. Hledáme matematické optimum ve velmi velkém souboru kontribučních faktorů. Tuto roli obrovsky usnadňuje fakt, že generační obměna felines je z biologického hlediska rychlá, což zaručuje obdržení prověřitelných dat v aplikačně rozumném čase. Historické sociologické studie u lidské populace i úspěšné chovatelské snahy zacílené na jiné, mnohem složitější znaky, náš předpoklad plně validují: naše cesta nepředstavuje divoký vědecký experiment. Jde o pragmatickou aplikaci průmyslového *best practice* přístupu vytaženého do extrému.

### Institucionální ukotvení a Well-being
V A VIRTÙ nejsme izolovaní. Aktivně vyhledáváme synergii s organizacemi zabývajícími se well-beingem zvířat v nejširším možném smyslu – primárně se zaměřujeme na tělesa definující a dozorující chovné standardy. Jsme regulérními členy Českého svazu chovatelů koček (ČSCH). Aktivně a otevřeně komunikujeme s lokálními institucemi (státními i nestátními), jež jakkoli ovlivňují kynologický a felinologický život. V současnosti prozkoumáváme cesty pro hlubší přesah do mezinárodních legislativních a chovatelských struktur. Měníme přístup, nikoli zákon.

Dnes, po odchování **Generace 1** (přímých potomků Šipky), simulace a predikce splynuly v realitu. Koťata vykazují nadprůměrné sociální chování a rapidní intelektový rozvoj bez jakýchkoli známek psychické regrese či dřívější druhové agresivity. K uvolnění se aktuálně nabízí trio **Sokrates, Sofie a Sybila**. Tato zvířata jsou k dispozici jak kastrovaná (pro privátní sféru), tak k dalšímu chovu pod naším dohledem. Spolu se zvířetem poskytujeme plný genetický atest, detailní zprávu zvěrolékaře a doživotní konzultační support. **Platí neměnné pravidlo: bez výjimky jsou všechna naše zvířata čipovaná a jejich identifikační čipy centrálně zaregistrovány.**

V roce 2026 pak očekáváme přírůstek asi deseti nových jedinců, přičemž 3-4 nejkvalitnější si ponecháme v našem kmenovém chovu. Tato nadcházející Generace 2 již poprvé podstoupí poloautomatizovanou technologickou analýzu chování v poměru 50/50 k tradičnímu komisnímu hodnocení (board-based). V dlouhodobém horizontu nám tyto technologie umožní kočky monitorovat nepřetržitě a maximalizovat tak *well-being* těchto stvoření jakožto plnohodnotných intelektuálních partnerů v domácnosti.

### Sociální Dynamika: Fenomén "Trio"

Ačkoli jsou naše kočky perfektně samostatné (testováno při předávání jedinců z pětihlavého vrhu novým i zavedeným chovatelům – nikde se neobjevily u žádné ze stran sebemenší úzkostné příznaky), silně podporujeme chov ve smečce. 

Ze zkušeností vyplývá: **chovat kočky v méně než třech jedincích je plýtvání jejich (i vaším) životem.** Pořízení si **Tria** je ultimátní krok do sféry, kde kočičí dynamika teprve začne dávat skutečný kognitivní smysl. Plně socializovaná a emočně vyrovnaná skupina tří a více jedinců si je navzájem nepřetržitou zábavou, mentální výzvou a silnou tlupou. Jakmile se na tuto úroveň dynamiky adaptujete vy, takto sehraná smečka dokáže své majitele milovat hlubokým, nepochopitelně sofistikovaným způsobem - což potvrdí každý chovatel, který si ke své původně jediné kočce později pořídil smečku další.

### Etika "All-Good" a Nulová Dominance

V našem chovu nevystavujeme zvířata absolutně žádným negativním stimulům ze strany člověka. Veškerá motivace a rozvoj probíhá striktně přes pozitivní posilování. Tento přístup pro nás není jenom "nejlepší", vnímáme jej jako prokazatelně *jediný* etický způsob formování zvířat na naší kognitivní specifikaci. Naše zvířata nikdy nedostanou důvod pro "krizi důvěry" v člověka. Naopak – při interakcích je ze strany majitele neustále aktivně vybízena a trénována k tomu, aby nad člověkem v rámci hry uměla dominovat. Nechceme plachá a skromná zvířata. Právě plné, neproražené sebevědomí ve vnitřním i venkovním prostředí je naprostým předpokladem pro *self-safety* chování šelmy. 

V této etice je **povinen bezpodmínečně a bez výjimek dál pokračovat** každý budoucí schválený majitel. Člověk je navzdory svému sebevědomí pro kočku pořád ohromný pětidekagramový tvor – uplatňovat nad ní dominanci či jakoukoli negativitu je cesta do pekel. Chovatel si tak musí osvojit mentální vzorce operátora, které nebudou křížit s agresivitou či vynucováním autority, aby tento exkluzivně vybalancovaný mezigenerační vztah nenarušil.

Prakticky to mj. znamená naprostou minimalizaci či vyhnutí se tradiční přepravě v úzkých tvrdých přepravkách, pokud to zvíře nechce. Pokud při jakékoli situaci propuká u zvířete panika, je naší intelektuální povinností hledat jiné řešení, nikoli ho páčit násilím do stresu. Odbourání negativity ze strany majitele ovšem neznamená pěstovat ze zvířete laxního, rozmazleného jedince. Mluvíme striktně o odstranění "stresu, který si zvíře vyloží jako vaši osobní zradu". Z vás nesmí na kočku vyzařovat nic negativního – avšak sdílený stres ze vnějších okolností (skončíte spolu chyceni v prudkém dešti) naopak zvíře chápe jako společnou hrozbu. A pokud ji s vámi přestojí, masivně to prohloubí vaši pack-dynamiku, stejně jako když zmoknete s lidským přítelem. Zvíře prostě chápe, že "jste na jedné lodi".

### FAQ: Etika a Filozofie Projektu

**Je etické vytvářet zvířata, která budou mít nároky na psychické zacházení blížící se lidským?**
Tato otázka je a vždy bude předmětem probíhající diskuse, ale pro nás má naprosto čistou a jasnou odpověď odvozenou ze samostatné doktríny, nikoliv programu. Kočky zkrátka patří na seznam inteligentních druhů na Zemi. My jako moderní civilizace jsme je naopak ze svého aktivního světa tak trochu izolovaně vyhodili. Jsou cizorodá, "nepatří mezi nás". Hledíme jeden na druhého a fundamentálně nám k porozumění cosi chybí.

S kočkami totiž lze nesmírně snadno komunikovat emocionálně. Dokonale rozumějí našemu rozpoložení – asi nanejvýš podobně přesně jako dvouleté dítě. Pozorné dítě i kočka brilantně poznají, čemu zrovna věnujete pozornost a zda se pohybujete a mluvíte jako obvykle. Pokud vaše dikce odpovídá normálu, jsou spokojené. Záměrné odpojení se od zvířat nás naproti tomu učí jen destruktivní schopnosti postupného emocionálního odpojení ode všeho ostatního ve světě. A to je pro formálně inteligentní bytost zhoubné.

Momentální pnutí a nedůvěra vzniká jednoduše tím, že posuzujeme zvířata (jimž se historicky nedostávalo cílené kognitivní "výchovy" či "vzdělání") chladným, neférovým prizmatem "človeka technologického". Felis Sapiens proto nestaví pouze na chladné genetice zakladatelky rodu. Třebaže se vyhýbáme ezoterice a pompézním fiktivním theoriím objevujícím objevné, stejnou měrou vyvíjíme celkové behaviorální metodiky, etologické principy a samotné způsoby, *jak se s kočkami naučit žít a vztahovat se k nim*, abychom konečně realizovali jejich latentní intelektuální potenciál. Zda je to celkově etické? Samozřejmě. My nestavíme lidi v chlupatém těle. Prostřednictvím poctivého pochopení koček se ve skutečnosti jen učíme to, jak znovuobjevovat ztracenou přirozenou sounáležitost se zvířaty obecně.

### FAQ: Praktické Otázky Zájemců
**O jaké kočičí plemeno se v programu vlastně jedná?**
Jedná se o Evropskou krátkosrstou kočku (European Shorthair), tedy de facto o nejběžnější přirozený druh kočky v rámci Evropy. Zakládající, mimořádně inteligentní samice našeho chovu ("Šipka") patřila právě k tomuto plemeni, takže na samotném počátku nebyl v podstatě žádný jiný prostor pro výběr. Ačkoliv je jasným faktem, že fantastickými etologickými a sociálními rysy disponují i některá jiná plemena, šlechtění dalšího extrémního IQ atributu nad jiným – mnohdy už tak příliš kosmeticky přešlechtěným plemenem – by z řady technických i etických ohledů nedávalo pražádný smysl. Jakékoliv křížení s jinou rasou v plánu není a nebude. Evropská krátkosrstá kočka navíc disponuje mimořádně robustním a přirozeným zdravím, není omezena patologickými estetickými znaky a je adaptibilním, špičkovým lovcem. Účelu posloužila naprosto fenomenálně.

**Vyžadují zvířata s tímto kognitivním potenciálem nějakou vysoce specifickou a drahou dietu?**
Absolutně ne. Kočky ke svému správnému fungování nevyžadují vůbec žádnou esoterickou či složitě odměřovanou zázračnou dietu, která by přesahovala běžné chovatelské *best practices*. Pro dosažení optimálního zdraví mozku a těla zkrátka vyžadujeme krmení vysoce prémiovou stravou bez levných obilných náhražek – jak to vyžaduje každý odpovědný chovatel –, ale nevyužitkujeme žádná přehnaně specifická dietní kouzla. 

**Používáte v chovném programu metody genetické manipulace?**
V žádném případě ne. Ačkoli proti formám přesné úpravy genů nejsme apriori principiálně zaujatí, pro naše stávající a nejbližší připravované generace nic takového vůbec nezvažujeme, protože pro nás hraje roli absolutně prokázaná bezpečnost pro zvířata. Je pravděpodobné, že pro uchování unikátní kognitivní informace zakladatelů rodu v budoucnu sáhneme po metodách konzervace genetického materiálu. Avšak možnosti současného komerčního klonování zvířat a jeho spolehlivá etická obhajitelnost (obzvlášť k roku 2026) zůstávají i nadále vysoce diskutabilní. Je to sice fascinující směr uvažování a teoretického i praktického výzkumu, který plně respektujeme, ovšem sami do této praxe dnes nevstupujeme.

**Jaký je zdravotní stav a veterinární historie zvířat, která předáváte do adopce?**
Zcela bezchybný. Veškerá zvířata, která náš chov opouštějí k soukromým zájemcům, neprodělala v minulosti naprosto žádnou chorobu, infekci ani sebemenší zranění. Jsou pochopitelně od narození pravidelně veterinárně kontrolována, očkována a ošetřována v přísném souladu se standardními medicínskými postupy v chovu koček. S každým předaným jedincem vždy odchází i jeho kompletní, reálná veterinární dokumentace, která tento bezvadný vývoj jasně fixuje.

**O kolik víc mě bude stát chov tří, čtyř nebo pěti koček namísto jedné?**
Finanční náklady stran prémiové stravy a veterinární obsluhy samozřejmě rostou víceméně lineárně. Ovšem mentální a kognitivní ROI (návratnost investice) roste exponenciálně obrovským tempem. Ve třech a více zvířatech naopak výrazně klesají vaše "náklady na neuro-obsluhu", protože zvířata nezávisí jen na vaší pozornosti, nýbrž fungují jako provázaná společnost.

**Mohu s takovou kočkou aktivně cestovat?**
Současná zvířata nikdy nebyla prověřována na každodenní zběsilé cestování. Udržíte-li však co nejtišší a nejklidnější prostředí, aniž byste zvíře uzavřeli bez komunikace s člověkem, budou schopna se výborně adaptovat. Z našich dřívějších zkušeností zvládala zvířata i celodenní jízdu autem a cizí týdenní pobyty v zahraničí bez nadměrné úzkosti. 
Cestování je pro ně samozřejmě náročnější než domov, ovšem pokud stoprocentně dodržíme pravidlo absolutní důvěry ("be good"), zvíře to zdravě unaví a nabije zcela novými vlivy. Tato investice do zážitků se později masivně odrazí v sebevědomějším vystupování zvířete zpět doma. Hlavní měřítko zní: neberte kočku nikam, kam byste s klidem nevzali dvouleté dítě.

**Mohu takovou kočku po převzetí přejmenovat?**
Z našeho vysoce subjektivního a emocionálního pohledu to nesledujeme rádi. Zvířatům jsme pečlivě vybrali zvučná jména po význačných historických osobnostech či postavách a pevně věříme, že jim dokonale sluší. Pokud ovšem tento čistě lidský sentiment opustíme a budeme se řídit výhradně objektivním *well-beingem* zvířat, neexistuje naprosto žádný empirický argument v tom smyslu, že by přejmenování kočkám jakkoli vadilo nebo je poškozovalo. Koneckonců, ani nám lidem zpravidla vůbec nevadí, když si pro nás naši blízcí vymyslí novou laskavou přezdívku. 

**Objeví se zvířata z vašeho chovu na výstavách?**
Rozhodně ano. Zvířata musí dle etického protokolu projít reálnými fenotypovými testy u komisařů k ověření, že vedle extrémní kognice striktně dodržujeme plné fyzické standardy plemene. Nejezdíme na výstavy extenzivně a už vůbec ne proto, abychom tam koťata z klecí nabízeli veřejnosti. Naše vzácná účast má legislativní a srovnávací charakter. Před každou plánovanou výstavou zašleme včas upozornění registrovaným zájemcům v našem systému.

**Nechci se momentálně stát majitelem vaší kočky, ale rád bych projekt z povzdálí sledoval. Jak na to?**
Vyplňte formální registrační rozhraní na webu s volbou "Newsletteru". Očekávejte ale nulový vnější nátlak: naše informační svodky odesíláme obvykle v taktu cca jednou za 6 měsíců. Chov koček i jejich etologie je v ideálním případě klidná, tichá záležitost. Náš ekosystém tímto pravidlem žije – komunikační tón u nás zůstává výlučně klidný, líný a spokojený. 

**Lze s vámi konzultovat vlastní chov a etologii?**
Máme přirozený zájem o veškeré postřehy a výměnu zkušeností. Zůstáváme nicméně obrovsky pokorní, co se týče našich vlastních znalostí – v žádném případě se nepovažujeme za "univerzální experty" na kočky. Naopak jsme neustále otevřeni novým úhlům pohledů. Pokud však bude váš konkrétní dotaz nebo problém spadat do našich technických či etologických kompetencí, velmi rádi a ochotně ho s vámi prodiskutujeme. Rádi vás uslyšíme. K zahájení komunikace neváhejte odeslat zkoumavý případ přes [inquiry formulář](prezentace/index.html#dossier-form).

### Výzkumná participace a Osobní Správce Chovu (Dashboard)
Zájemcům, kteří projdou prvotním Inquiry registrem, se bezplatně zpřístupní náš izolovaný **Osobní správce chovu (Dashboard)**. Tento zabezpečený cloudový software obsahuje detailní dotazníky a kalkulátory pro stanovení behaviorálního kognitivního skóre jejich vlastních nebo budoucích zvířat. Celý nástroj je striktně *technický a analytický*, určen výhradně pro vážné účely datové kalibrace – nikoli pro zábavu komerční veřejnosti. Není naším záměrem umožnit lidem jen tak ze srandy diagnostikovat, že jejich "Míca odvedle" má obrovské IQ, a podepisovat se pod to jako falešná autorita.

Aktivně do tohoto rozhraní naopak hledáme **dobrovolníky z řad stávajících zavedených chovatelů**. Jejich kočky sice nepocházejí z naší liniové selekce, ale pokud kočky kognitivně sledují a zadají do systému po občasném vyplnění dotazníků jejich profily, tato validní data nám poslouží k masivnímu zpřesnění a tréninku našich simulačních algoritmů. Dobrovolník při registraci dává souhlas s anonymním zpracováním těchto metrik. 
Jako poděkování systém volitelně nabízí **kreditaci jména či značky chovatele**. Pokud chovatel souhlasí, jeho jméno nebo nahráté logo chovatelské stanice se transparentně zobrazí dole na zdi webové prezentace v sekci "Research Calibration Partners". Tento souhlas (stejně jako samotnou participaci ve sdílení dat) může uživatel ve svém Dashboard účtu samozřejmě *kdykoli a jedním kliknutím odvolat*, čímž dojde k okamžitému výmazu jeho poděkování i profilování.

My zvířata běžně převážíme mezi městským zázemím a testovacím venkovským prostorem (vybaveným sledovací a automatizační technikou). Kočky samotnou cestu jásotem zrovna nevítají. Zpočátku si trochu vokálně stěžují, ale rozhodně nepropadají panice a následně prostě usnou. Jak dorazíme, stačí jim otevřít dveře, ukázat misku, a ony se s absolutní drzou suverenitou rychle rozkoukají.
Důležité varování: projev kognice našich zvířat znamená i to, že neomylně chápou přípravy na cestu a ihned se instinktivně schovají. Plánujte dostatek klidného času na jejich "sběr" z bytu. Čím větší stres u vás vycítí, tím geniálnější úkryty najdou. I když moc dobře znají svá jména, na pouhé zavolání zásadně nepřijdou.

## Chovná Kniha a Generace 01
Veškeré podrobnosti o zakládajícím vrhu 5 jedinců (Sokrates, Sofie, Sibyla, Zen a Aténa), jejich sociální dynamice a detailních povahových rysech, jsou zaznamenávány v oddělené [Chovné knize (Stud Book)](metodika/chovna-kniha.md).
