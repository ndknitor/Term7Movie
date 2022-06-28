import 'package:term7moviemobile/models/theater_model.dart';

class CompanyModel {
  int? id;
  bool? isActive;
  String? logoUrl;
  String? managerEmail;
  int? managerId;
  String? managerName;
  String? name;
  List<TheaterModel>? theaters;

  CompanyModel(
      {this.id,
        this.isActive,
        this.logoUrl,
        this.managerEmail,
        this.managerId,
        this.managerName,
        this.name, this.theaters});

  CompanyModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    isActive = json['isActive'];
    logoUrl = json['logoUrl'];
    managerEmail = json['managerEmail'];
    managerId = json['managerId'];
    managerName = json['managerName'];
    name = json['name'];
    if (json['theaters'] != null) {
      theaters = <TheaterModel>[];
      json['theaters'].forEach((v) {
        theaters!.add(new TheaterModel.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['isActive'] = this.isActive;
    data['logoUrl'] = this.logoUrl;
    data['managerEmail'] = this.managerEmail;
    data['managerId'] = this.managerId;
    data['managerName'] = this.managerName;
    data['name'] = this.name;
    if (this.theaters != null) {
      data['theaters'] = this.theaters!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}