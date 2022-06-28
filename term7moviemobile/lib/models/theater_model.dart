class TheaterModel {
  int? id;
  String? address;
  String? latitude;
  String? longitude;
  bool? status;
  int? totalRoom;
  String? name;

  TheaterModel(
      {this.id,
        this.address,
        this.latitude,
        this.longitude,
        this.status,
        this.totalRoom,
        this.name});

  TheaterModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    address = json['address'];
    latitude = json['latitude'];
    longitude = json['longitude'];
    status = json['status'];
    totalRoom = json['totalRoom'];
    name = json['name'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['address'] = this.address;
    data['latitude'] = this.latitude;
    data['longitude'] = this.longitude;
    data['status'] = this.status;
    data['totalRoom'] = this.totalRoom;
    data['name'] = this.name;
    return data;
  }
}