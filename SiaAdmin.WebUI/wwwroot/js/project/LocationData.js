// Türkiye illeri, ilçeleri ve bölgeleri için statik data
var LocationData = {
    // İller
    provinces: [
        { id: 1, name: "Adana" },
        { id: 2, name: "Adıyaman" },
        { id: 3, name: "Afyonkarahisar" },
        { id: 4, name: "Ağrı" },
        { id: 5, name: "Amasya" },
        { id: 6, name: "Ankara" },
        { id: 7, name: "Antalya" },
        { id: 8, name: "Artvin" },
        { id: 9, name: "Aydın" },
        { id: 10, name: "Balıkesir" },
        { id: 11, name: "Bilecik" },
        { id: 12, name: "Bingöl" },
        { id: 13, name: "Bitlis" },
        { id: 14, name: "Bolu" },
        { id: 15, name: "Burdur" },
        { id: 16, name: "Bursa" },
        { id: 17, name: "Çanakkale" },
        { id: 18, name: "Çankırı" },
        { id: 19, name: "Çorum" },
        { id: 20, name: "Denizli" },
        { id: 21, name: "Diyarbakır" },
        { id: 22, name: "Edirne" },
        { id: 23, name: "Elazığ" },
        { id: 24, name: "Erzincan" },
        { id: 25, name: "Erzurum" },
        { id: 26, name: "Eskişehir" },
        { id: 27, name: "Gaziantep" },
        { id: 28, name: "Giresun" },
        { id: 29, name: "Gümüşhane" },
        { id: 30, name: "Hakkari" },
        { id: 31, name: "Hatay" },
        { id: 32, name: "Isparta" },
        { id: 33, name: "Mersin" },
        { id: 34, name: "İstanbul" },
        { id: 35, name: "İzmir" },
        { id: 36, name: "Kars" },
        { id: 37, name: "Kastamonu" },
        { id: 38, name: "Kayseri" },
        { id: 39, name: "Kırklareli" },
        { id: 40, name: "Kırşehir" },
        { id: 41, name: "Kocaeli" },
        { id: 42, name: "Konya" },
        { id: 43, name: "Kütahya" },
        { id: 44, name: "Malatya" },
        { id: 45, name: "Manisa" },
        { id: 46, name: "Kahramanmaraş" },
        { id: 47, name: "Mardin" },
        { id: 48, name: "Muğla" },
        { id: 49, name: "Muş" },
        { id: 50, name: "Nevşehir" },
        { id: 51, name: "Niğde" },
        { id: 52, name: "Ordu" },
        { id: 53, name: "Rize" },
        { id: 54, name: "Sakarya" },
        { id: 55, name: "Samsun" },
        { id: 56, name: "Siirt" },
        { id: 57, name: "Sinop" },
        { id: 58, name: "Sivas" },
        { id: 59, name: "Tekirdağ" },
        { id: 60, name: "Tokat" },
        { id: 61, name: "Trabzon" },
        { id: 62, name: "Tunceli" },
        { id: 63, name: "Şanlıurfa" },
        { id: 64, name: "Uşak" },
        { id: 65, name: "Van" },
        { id: 66, name: "Yozgat" },
        { id: 67, name: "Zonguldak" },
        { id: 68, name: "Aksaray" },
        { id: 69, name: "Bayburt" },
        { id: 70, name: "Karaman" },
        { id: 71, name: "Kırıkkale" },
        { id: 72, name: "Batman" },
        { id: 73, name: "Şırnak" },
        { id: 74, name: "Bartın" },
        { id: 75, name: "Ardahan" },
        { id: 76, name: "Iğdır" },
        { id: 77, name: "Yalova" },
        { id: 78, name: "Karabük" },
        { id: 79, name: "Kilis" },
        { id: 80, name: "Osmaniye" },
        { id: 81, name: "Düzce" }
    ],

 

    // Türkiye'nin 7 bölgesi
    regions: [
        { id: 1, name: "TR1 - İstanbul" },
        { id: 2, name: "TR2 - Batı Marmara" },
        { id: 3, name: "TR3 - Ege" },
        { id: 4, name: "TR4 - Doğu Marmara" },
        { id: 5, name: "TR5 - Batı Anadolu" },
        { id: 6, name: "TR6 - Akdeniz" },
        { id: 7, name: "TR7 - Orta Anadolu" },
        { id: 8, name: "TR8 - Batı Karadeniz" },
        { id: 9, name: "TR9 - Doğu Karadeniz" },
        { id: 10, name: "TRA - Kuzeydoğu Anadolu" },
        { id: 11, name: "TRB - Ortadoğu Anadolu" },
        { id: 12, name: "TRC - Güneydoğu Anadolu" },
    ],

    // İllerin bağlı olduğu bölgeler
    provinceRegions: {
        // Akdeniz Bölgesi
        1: [1, 7, 15, 31, 32, 33, 46, 80],
        // Doğu Anadolu Bölgesi
        2: [4, 12, 13, 23, 24, 25, 30, 36, 44, 49, 62, 65, 75, 76],
        // Ege Bölgesi
        3: [3, 9, 20, 35, 43, 45, 48, 64],
        // Güneydoğu Anadolu Bölgesi
        4: [2, 21, 27, 47, 56, 63, 72, 73, 79],
        // İç Anadolu Bölgesi
        5: [6, 18, 19, 26, 38, 40, 42, 50, 51, 58, 66, 68, 70, 71],
        // Karadeniz Bölgesi
        6: [5, 8, 14, 28, 29, 37, 52, 53, 55, 57, 60, 61, 67, 69, 74, 78, 81],
        // Marmara Bölgesi
        7: [10, 11, 16, 17, 22, 34, 39, 41, 54, 59, 77]
    },

    // Eğitim Seçenekleri
    educationOptions: [
        { id: 1, name: "Eğitimsiz" },
        { id: 2, name: "İlkokul" },
        { id: 3, name: "İlköğretim/Orta" },
        { id: 4, name: "Lise" },
        { id: 5, name: "Yüksekokul/Ünv" },
     
    ],

    // Meslek Grupları
    occupations: [
        { id: 1, name: "İşçi" },
        { id: 2, name: "Memur" },
        { id: 3, name: "Öğretmen" },
        { id: 4, name: "Doktor" },
        { id: 5, name: "Mühendis" },
        { id: 6, name: "Esnaf" },
        { id: 7, name: "Çiftçi" },
        { id: 8, name: "Ev Hanımı" },
        { id: 9, name: "Öğrenci" },
        { id: 10, name: "Emekli" },
        { id: 11, name: "Serbest Meslek" },
        { id: 12, name: "Diğer" }
    ]
};