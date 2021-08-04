# DiningPhilosophersExtended
Problem ucztujących filozofów jest przykładem klasycznego zdania synchronizacji procesów.
W tym przypadku mamy sytuację przypominającą ucztujących filozofów, lecz z dwoma modyfikacjami.

1. Filozofowie przy jedzeniu czytają. Mamy do dyspozycji _**k**_ książek i każdy przed rozpoczęciem jedzenia bierze sobie jedną z książek - za każdym razem inną.

2. Mogą za każdym razem usiąść przy innym miejscu. Widelce biorą zawsze z sąsiedztwa wybranego nakrycia, ale miejsce wybierają również losowo.

Do rozwiązania problemu zastosowana została klasa __Monitor__, która znacząco wpływa na wzrost wydajności programu, dzięki synchronizacji wyłącznie tego bloku kodu, który tego potrzebuje.
