# 🛡️ C# Portfolio | Alessio Attilia

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/download)
[![Security](https://img.shields.io/badge/Security-Focus-red?style=for-the-badge&logo=codacy&logoColor=white)](#)


Benvenuti nel mio spazio di sviluppo. Questo repository raccoglie moduli ingegneristici in C# progettati secondo il paradigma della **Security by Design**. Ogni esercizio non è una semplice esecuzione di algoritmi, ma una dimostrazione di come gestire dati sensibili, prevenire crash di sistema e validare input in ambienti critici.

---

## 🏛️ Architettura e Metodologia

Il mio approccio integra i principi del **Model-Driven Engineering (MDE)** per garantire l'affidabilità strutturale del software. Utilizzo sistematicamente strumenti di **AI Generativa (Prompt Engineering)** per il "rubber ducking" tecnico, ottimizzando la logica di debugging e la pulizia del codice.



---

## 📂 Moduli Tecnici Disponibili

### 1. Threat & Network Analyzer 🌐
**Focus: Gestione delle Collezioni e Validazione Protocolli**
Questo modulo simula un sistema di monitoraggio del traffico di rete.
* **Blacklist Filter**: Ricerca ad alte prestazioni su Array di IP malevoli.
* **Quarantine System**: Manipolazione sicura di liste di minacce tramite cicli for inversi (prevenzione crash durante la rimozione di elementi).
* **Attack Mapping**: Utilizzo di `Dictionary` per il tracciamento in tempo reale della frequenza degli attacchi per IP.
* **Protocol Integrity**: Validazione rigorosa degli indirizzi IPv4 tramite parsing degli ottetti.

### 2. AIScanner & Anomaly Scorer 🤖
**Focus: Matrici e Safe Parsing**
Analisi di matrici di server per il rilevamento di porte aperte e classificazione della gravità delle minacce.
* **Matrix Scan**: Scansione dinamica di matrici multidimensionali con controllo dei limiti (Out-of-Bounds prevention).
* **Severity Evaluation**: Algoritmi di scoring che utilizzano `double.TryParse` per prevenire eccezioni di formattazione durante l'input utente.

### 3. Data Cleaning Pipeline 🧹
**Focus: Sanitizzazione Input e Nullable Types**
Simulazione di un backend che riceve dati sporchi o incompleti.
* **Sanitizzazione Estrema**: Pulizia di stringhe (trim, null-check, literal-check) per garantire l'integrità dei dati nel database.
* **Lockout Logic**: Utilizzo di **Nullable Types** (`int?`) per gestire in modo sicuro i tentativi di login falliti, prevenendo `NullReferenceException`.
* **Log Extraction**: Parsing di stringhe strutturate (split log) con validazione della lunghezza dell'array.

### 4. Secure Logs Auditor 📑
**Focus: File System I/O e Resource Management**
Un sistema di auditing che scrive, legge e archivia eventi di sicurezza su disco.
* **Resource Handling**: Utilizzo dello statement `using` per garantire il rilascio immediato degli handle dei file.
* **Error Auditing**: Gestione granulare delle eccezioni (`UnauthorizedAccessException`, `IOException`) per evitare fughe di informazioni o crash di sistema.
* **File Sharing Logic**: Configurazione di `FileShare.ReadWrite` per permettere l'archiviazione dei log anche durante la scrittura attiva.

---

## 🛠️ Security Highlights (Codice Blindato)

| Funzionalità | Rischio Prevenuto | Tecnica Utilizzata |
| :--- | :--- | :--- |
| **Input Utente** | Code Injection / Format Error | `TryParse` e `.Trim()` sistematico. |
| **Rimozione Elementi** | IndexOutOfRangeException | Ciclo `for` decrementale. |
| **Accesso File** | File Lock / Denied Access | Blocchi `try-catch` e `FileShare` mode. |
| **Stato Variabili** | Inconsistenza Dati | `Nullable Types` con controllo `.HasValue`. |

---

## 🔗 Collegamenti Rapidi
* **[🌐 Portfolio Web (Stile IDE)](https://alessioattilia03-gif.github.io)**
* **[📄 Download CV Completo](https://alessioattilia03-gif.github.io/cv_professional.pdf)**
* **[💼 Profilo LinkedIn](https://linkedin.com/in/alessio-attilia)**

---
*Progettato con rigore ingegneristico. Ultimo aggiornamento: Marzo 2026.*

---
*Ultimo aggiornamento: Marzo 2026. Sviluppato con passione e precisione.*
