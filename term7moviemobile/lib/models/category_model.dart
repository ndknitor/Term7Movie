class Categories {
  int? cateId;
  String? cateName;

  Categories({this.cateId, this.cateName});

  Categories.fromJson(Map<String, dynamic> json) {
    cateId = json['cateId'];
    cateName = json['cateName'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['cateId'] = this.cateId;
    data['cateName'] = this.cateName;
    return data;
  }
}