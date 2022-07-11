import 'package:flutter/widgets.dart';

class Categories {
  int? id;
  String? name;
  String? color;

  Categories({this.id, this.name, this.color});

  Categories.fromJson(Map<String, dynamic> json) {
    id = json['cateId'] ?? json['id'];
    name = json['cateName'] ?? json['name'];
    color = json['cateColor'] ?? json['color'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['cateId'] = this.id;
    data['cateName'] = this.name;
    data['cateColor'] = this.color;
    return data;
  }
}