# Beschreibung

Es gibt eine OpenCore-Instanz, die über Queues mit 4 HD-Robots über OpenRPA kommunziert. Dabei kann es zu Verbindungsabbrüchen zwischen OpenRPA und OpenCore bzw. OpenRPA und der Queue kommen. OpenRPA bearbeitet sein letztes Workitem daraufhin noch weiter, kann aber seinen Status bei OpenCore nicht mehr aktualisieren. Für OpenCore ist der Bot nun für immer beschäftigt und weist ihm keine neue Aufgabe zu.

# User Beschreibung

1) OpenRpa auf der Windows VM zieht sich mit dem HDBotUser ein Workitem
2) Irgendwie bricht die Verbindung zwischen OpenCore und OpenRPA ab
3) Der HD Bot zieht sich selbst anch recconect kein workitem mehr

# Gewünschtes Verhalten

OpenRPA soll nach verlorener Verbindung zu OpenCore bzw. zur Queue selbstständig die Verbindung wiederherstellen, sobald wie möglich. Es reicht, wenn jede Minute versucht wird eine neue Verbindung herzustellen. Dafür soll OpenRPA eine verlorene Verbindung erkennen und den Status des letzten Workitems behalten, sodass OpenRPA bei einer Wiederaufnahme der Verbindung zu OpenRPA bzw. zur Queue den Status des Worktitems korrekt übermitteln kann.

# Logs
```
[19:43:36.724][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:43:25.409][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:43:21.721][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:43:10.405][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:43:06.716][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:43:05.571][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:55.412][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:42:51.729][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:50.571][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:40.320][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:42:36.641][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:35.562][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:25.310][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:42:21.636][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:20.572][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:10.240][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:42:06.628][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:42:05.565][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:55.236][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:41:51.624][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:50.572][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:40.242][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:41:36.537][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:35.568][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:25.239][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:41:21.533][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:20.564][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:10.235][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:41:06.449][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:41:05.557][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:55.243][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:40:51.460][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:50.552][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:40.238][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:40:36.455][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:35.547][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:25.234][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:40:21.450][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:20.543][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:10.229][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:40:06.461][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:40:05.554][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:55.224][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:39:51.456][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:50.548][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:40.220][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:39:36.452][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:35.545][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:25.215][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:39:21.448][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:20.457][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:10.222][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:39:06.454][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:39:05.453][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:55.217][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:38:51.449][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:50.448][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:40.213][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:38:36.461][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:35.460][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:25.202][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:38:21.451][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:20.371][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:10.198][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:38:06.447][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:38:05.383][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:55.210][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:37:51.443][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:50.379][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:40.199][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:37:36.432][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:35.368][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:25.127][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:37:21.439][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:20.359][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:10.122][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:37:06.435][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:37:05.356][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:55.118][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:36:51.444][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:50.286][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:40.025][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:36:36.430][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:35.288][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:25.037][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:36:21.337][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:20.288][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:10.037][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:36:06.325][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:36:05.277][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:54.994][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:35:51.321][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:50.288][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:40.007][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:35:36.234][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:35.278][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:24.996][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:35:21.229][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:20.274][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:09.924][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:35:06.236][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:35:05.265][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:54.920][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:34:51.149][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:50.257][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:39.927][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:34:36.160][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:35.252][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:24.923][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:34:21.155][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:20.247][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:09.917][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:34:06.036][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:34:05.254][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:54.924][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:33:51.030][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:50.247][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:39.918][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:33:36.025][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:35.242][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:24.913][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:33:21.037][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:20.256][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:09.909][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:33:06.033][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:33:05.250][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:54.905][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:32:51.027][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:50.245][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:39.900][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:32:36.024][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:35.256][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:24.910][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:32:21.019][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:20.251][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:09.905][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:32:06.013][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:32:05.246][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:54.900][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:31:51.009][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:50.241][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:39.910][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:31:36.019][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:35.236][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:24.904][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:31:21.011][Warning] Message f24b2f76-fe31-4226-847f-2f683fba0886 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:20.149][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:31:09.897][Warning] Message 3e42fd5c-72e4-46d5-9f0c-147d73a583c2 (insertorupdateone) state failed timed out, retrying state: 
[19:31:06.864][Output] Main completed in 01:28.929
[19:31:06.858][Information] Main resumed
[19:31:06.488][Output] CloseAllApplications completed in 00:00.463
[19:31:06.483][Output] Excel Already Closed
[19:31:06.375][Output] Word Already Closed
[19:31:06.025][Output] Closing applications...
[19:31:06.023][Information] CloseAllApplications started in 00:00.000
[19:31:05.923][Output] Process finished due to no more transaction data
[19:31:05.919][Output] Error getting transaction data for Transaction Number: 0. Gave up on 079e0fce-3905-4dd2-a05f-bc5e97b1ec3d popworkitem at Source: OpenRPA.Net
[19:31:05.832][Output] Could not retrieve transaction item. Exception message: Gave up on 079e0fce-3905-4dd2-a05f-bc5e97b1ec3d popworkitem
[19:31:05.829][Error] WebSocketClient.SendMessage: Not connected/signed in to OpenFlow
[19:31:05.829][Error] Gave up on 079e0fce-3905-4dd2-a05f-bc5e97b1ec3d popworkitem not connected
[19:31:05.829][Warning] Message 079e0fce-3905-4dd2-a05f-bc5e97b1ec3d (popworkitem) timed out, retrying
[19:31:05.156][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:30:50.733][Output] Get the transaction item
[19:30:50.728][Output] SetTransactionStatus.xaml failed: Carlo Auftragsbestätigung/SetTransactionStatus failed with Gave up on e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc updateworkitem at Source: 1.54
[19:30:50.642][Error] System.Exception: Carlo Auftragsbestätigung/SetTransactionStatus failed with Gave up on e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc updateworkitem ---> System.Exception: Gave up on e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc updateworkitem
   bei OpenRPA.Net.WebSocketClient.<SendMessage>d__62.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   bei System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   bei OpenRPA.Net.SocketCommand.<SendMessage>d__24`1.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   bei System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   bei OpenRPA.Net.WebSocketClient.<UpdateWorkitem>d__101`1.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   bei System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   bei OpenRPA.WorkItems.UpdateWorkitem.<ExecuteAsync>d__28.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei OpenRPA.Interfaces.AsyncTaskCodeActivity.EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
   bei System.Activities.AsyncCodeActivity.CompleteAsyncCodeActivityData.CompleteAsyncCodeActivityWorkItem.Execute(ActivityExecutor executor, BookmarkManager bookmarkManager)
   --- Ende der internen Ausnahmestapelüberwachung ---
   bei OpenRPA.Activities.InvokeOpenRPA.OnBookmarkCallback(NativeActivityContext context, Bookmark bookmark, Object obj)
[19:30:50.641][Information] Main resumed
[19:30:50.353][Output] SetTransactionStatus failed at 1.54 in 00:15.189
# Gave up on e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc updateworkitem
[19:30:50.349][Output] Could not set the transaction status. Gave up on e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc updateworkitem At Source: OpenRPA.Net
[19:30:50.347][Error] WebSocketClient.SendMessage: Not connected/signed in to OpenFlow
[19:30:50.347][Error] Gave up on e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc updateworkitem not connected
[19:30:50.262][Warning] Message e6d41b43-8eab-478e-8f9d-5c64bcb7a8fc (updateworkitem) timed out, retrying
[19:30:50.150][Warning] Message 2dcb7561-5549-4295-8d31-7f80110306f3 (insertorupdateone) state loaded timed out, retrying state: 
[19:30:35.287][Error] System.ArgumentException: Das Zielarray ist nicht lang genug. Überprüfen Sie destIndex, die Länge und die Untergrenze des Arrays.
   bei System.Array.Copy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length, Boolean reliable)
   bei System.Collections.Generic.List`1.CopyTo(T[] array, Int32 arrayIndex)
   bei System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   bei System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   bei OpenRPA.Net.WebSocketClient.Process(Message msg)
   bei OpenRPA.Net.WebSocketClient.<ProcessQueue>d__58.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   bei System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   bei OpenRPA.Net.WebSocketClient.<receiveLoop>d__56.MoveNext()
[19:30:35.261][Error] {"id":"f5248587f54b72e3","replyto":"2dcb7561-5549-4295-8d31-7f80110306f3","command":"insertorupdateone","data":"{\"w\":0,\"j\":false,\"uniqeness\":null,\"collectionname\":\"openrpa_instances\",\"result\":{\"Bookmarks\":null,\"correlationId\":null,\"queuename\":null,\"console\":null,\"InstanceId\":null,\"WorkflowId\":\"07ccc2a4-9e55-460a-8258-4f9ca30db4ea\",\"caller\":null,\"RelativeFilename\":\"Carlo Auftragsbestätigung/SetTransactionStatus.xaml\",\"projectid\":\"683ff6846e5ef7706f91c425\",\"projectname\":\"Carlo Auftragsbestätigung\",\"xml\":null,\"owner\":\"ServiceUserRPA1\",\"ownerid\":\"685b922a02117c0c7d762cda\",\"host\":\"949dcrpats1\",\"fqdn\":\"949dcrpats1.avag.holding\",\"errormessage\":null,\"errorsource\":null,\"ident\":1,\"isCompleted\":false,\"hasError\":false,\"state\":\"loaded\",\"parentspanid\":null,\"spanid\":null,\"TraceId\":null,\"_id\":\"2ae5745bd8844434ba0d1f5158fd36ee\",\"_type\":\"workflowinstance\",\"name\":\"SetTransactionStatus\",\"_modified\":\"2025-08-05T17:30:35.163Z\",\"_modifiedby\":\"ServiceUserRPA1\",\"_modifiedbyid\":\"685b922a02117c0c7d762cda\",\"_created\":\"2025-08-05T17:30:35.163Z\",\"_createdby\":\"ServiceUserRPA1\",\"_createdbyid\":\"685b922a02117c0c7d762cda\",\"_acl\":[{\"deny\":null,\"rights\":65535,\"_id\":\"5a1702fa245d9013697656fb\",\"name\":\"admins\"},{\"deny\":null,\"rights\":65523,\"_id\":\"685a676902117c0c7d761a43\",\"name\":\"HDRobot\"},{\"rights\":65535,\"_id\":\"685b922a02117c0c7d762cda\",\"name\":\"ServiceUserRPA1\"}],\"_encrypt\":null,\"_version\":0},\"traceId\":\"\",\"spanId\":\"\",\"error\":null}","count":1,"index":0,"priority":1}
[19:30:35.161][Information] SetTransactionStatus started in 00:00.000
[19:30:35.060][Information] Main resumed
[19:30:34.773][Output] Process failed at 1.8 in 00:06.363
# Artikel konnte in beiden FCA Plattformen nicht gefunden werden. Bestellung wird später wiederholt.
[19:30:34.564][Information] Process resumed
[19:30:34.362][Output] Process FCA failed at 1.68 in 00:05.752
# Artikel konnte in beiden FCA Plattformen nicht gefunden werden. Bestellung wird später wiederholt.
[19:30:34.158][Warning] Cannot invoke Main, I'm busy. (running Carlo Auftragsbestätigung/Main)
[19:30:34.061][Warning] Cannot invoke Main, I'm busy. (running Carlo Auftragsbestätigung/Main)
[19:30:28.612][Information] Process FCA started in 00:00.003
[19:30:28.409][Output] Started Process
[19:30:28.408][Information] Process started in 00:00.000
[19:30:28.310][Output] Processing Transaction Number: 0
[19:30:28.218][Output] retrieved new workitem
[19:30:28.112][Output] Get the transaction item
[19:30:28.011][Output] SetTransactionStatus.xaml failed: Carlo Auftragsbestätigung/SetTransactionStatus failed with server error: {"message":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found.","error":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found."} at Source: 1.54
[19:30:28.009][Error] System.Exception: Carlo Auftragsbestätigung/SetTransactionStatus failed with server error: {"message":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found.","error":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found."} ---> OpenRPA.Interfaces.SocketException: server error: {"message":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found.","error":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found."}
   bei OpenRPA.Net.SocketCommand.<SendMessage>d__24`1.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   bei System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   bei OpenRPA.Net.WebSocketClient.<UpdateWorkitem>d__101`1.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   bei System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   bei OpenRPA.WorkItems.UpdateWorkitem.<ExecuteAsync>d__28.MoveNext()
--- Ende der Stapelüberwachung vom vorhergehenden Ort, an dem die Ausnahme ausgelöst wurde ---
   bei System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   bei OpenRPA.Interfaces.AsyncTaskCodeActivity.EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
   bei System.Activities.AsyncCodeActivity.CompleteAsyncCodeActivityData.CompleteAsyncCodeActivityWorkItem.Execute(ActivityExecutor executor, BookmarkManager bookmarkManager)
   --- Ende der internen Ausnahmestapelüberwachung ---
   bei OpenRPA.Activities.InvokeOpenRPA.OnBookmarkCallback(NativeActivityContext context, Bookmark bookmark, Object obj)
[19:30:28.009][Information] Main resumed
[19:30:27.706][Output] SetTransactionStatus failed at 1.54 in 00:00.291
# server error: {"message":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found.","error":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found."}
[19:30:27.702][Output] Could not set the transaction status. server error: {"message":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found.","error":"Work item queue not found Carlo Auftragsbestätigung_Failure (688223d36e761c9f76bc5e5d) not found."} At Source: OpenRPA.Net
[19:30:27.413][Information] SetTransactionStatus started in 00:00.000
[19:30:27.319][Information] Main resumed
[19:30:27.019][Output] Process failed at 1.8 in 00:06.492
```
