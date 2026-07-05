# Turn-Based RPG Mini-Game (Toge Productions Pre-Interview Test)

Proyek ini adalah purwarupa (prototype) mini-game Turn-Based RPG JRPG-Style yang dibangun di Unity sebagai bagian dari Pre-Interview Test untuk posisi Programmer di Toge Productions. Pengembangan proyek ini berfokus pada pembangunan arsitektur kode yang bersih, modular, dapat dikembangkan (scalable), serta efisien dalam manajemen memori.

---

## 📋 Fitur & Ketentuan Tugas (Requirement Checklist)

### 3D/2D Objects
- [x] **Player:** Representasi visual karakter utama di dunia luar dan arena.
- [x] **NPC:** Karakter non-pemain untuk elemen interaksi narasi.
- [x] **Enemy:** Unit lawan (Goblin) di Overworld dan Battle Arena.
- [x] **Background:** Lingkungan peta eksplorasi dan arena pertarungan.

### Gameplay & Mechanics
- [x] **Eksplorasi Dunia (World Map Locomotion):** Pergerakan Player menggunakan input WASD / Arrow Keys.
- [x] **Interaksi Objek:** Menggunakan tombol `Spacebar` untuk berinteraksi dengan NPC/objek di map.
- [x] **Sistem Dialog & Cutscene:** Pemanggilan dialog interaktif dan Skuens Sinematik/Cutscene di mana avatar player bergerak otomatis dikontrol penuh menggunakan integrasi **Fungus**.
- [x] **Sistem Pertempuran Turn-Based (1 vs 1):** Mekanik JRPG klasik dengan aksi *Basic Attack*, *Heavy Attack* (150% damage), *Heal* (Mantra pemulihan HP), dan *Run* (Dummy mechanic tiruan efek *Shadow Tag* Pokémon).

---

## 🛠️ Implementasi Teknis & Pola Arsitektur (Technical Architecture)

Proyek ini menghindari penulisan *God Class* (skrip raksasa) dan mengimplementasikan beberapa prinsip desain perangkat lunak standar industri:

### 1. Dual-Container Single-Scene Architecture
Untuk efisiensi waktu pengerjaan prototype tanpa mengorbankan stabilitas, game ini menggunakan pendekatan satu scene yang dibagi menjadi dua induk hierarki: `EXPLORATION_CONTAINER` dan `BATTLE_CONTAINER`. Transisi antar-fase dikendalikan secara mutlak melalui manipulasi `SetActive` terpusat dari `UIManager` yang dipicu di ujung blok perintah Fungus.

### 2. Finite State Machine (FSM) Terpusat via Coroutine
Alur turn-based pertempuran diatur secara linear menggunakan mesin status `enum BattleState` (`START`, `PLAYERTURN`, `ENEMYTURN`, `WON`, `LOST`). Pemindahan giliran dikelola menggunakan *Coroutine* (`IEnumerator`) untuk memastikan sekuens log, kalkulasi angka, animasi, dan pemblokiran input UI berjalan secara berurutan (*thread-safe input management*) guna mencegah eksploitasi *input spamming*.

### 3. Separation of Concerns (SoC) & Clean Code
Tanggung jawab logika diisolasi secara ketat ke dalam komponen-komponen terpisah:
* `BattleSystem`: Bertindak sebagai sutradara waktu (*Orchestrator*) yang mengatur alur pergantian State pertarungan.
* `BattleUnit`: Domain data runtime karakter. Komponen ini bertugas menampung variabel HP dan ATK lokal serta mengeksekusi fungsi konsekuensi damage (`TakeDamage`) dan pemulihan (`Heal`).
* `BattleUI`: Murni bertugas menangani representasi visual Canvas UI, pembaruan Slider bar darah, dan teks log pertarungan secara pasif.
* `BattleAudioManager`: Mengisolasi seluruh penanganan aset suara *One-Shot SFX* dan looping musik latar (*BGM Channels*).

### 4. Runtime Data Safety (Anti-SO Mutation Bug)
Karakteristik template status karakter disimpan di dalam `ScriptableObject`. Untuk mencegah bug permanen mutasi data aset asli pada disk saat berjalan di Unity Editor, skrip `BattleUnit` menyalin (*cloning*) nilai status tersebut ke dalam variabel lokal runtime melalui fungsi `SetupUnit()` setiap kali status pertempuran dimulai kembali.

### 5. Safe Singleton Pattern & Memory Cleanup
Guna mengatasi masalah *Race Condition* dan eror `MissingReferenceException` sisa referensi hantu (*ghost wrapper pointer*) pada variabel statik saat scene di-restart, kelas `BattleAudioManager` dilengkapi penanganan enkapsulasi properti *Getter* khusus serta interseptor pembersihan memori otomatis pada fungsi `OnDestroy()`.

### 6. Optimasi Audio Spesifik Platform WebGL
Pengaturan *Import Settings* audio dikonfigurasi secara manual untuk meminimalkan beban RAM browser:
* **Transient SFX (Pendek):** Menggunakan mode **Decompress On Load** dan format PCM/Vorbis untuk mencapai latensi absolut (0 ms) agar audio terasa responsif.
* **BGM / Musik Latar (Panjang):** Menggunakan mode **Compressed In Memory** dengan kompresi kualitas 70% guna menghemat memori RAM browser penilai agar build WebGL terhindar dari risiko *Out-of-Memory (OOM) Crash*.

---
