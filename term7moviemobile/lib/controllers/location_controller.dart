import 'package:geocoding/geocoding.dart' as geo;
import 'package:get/get.dart';
import 'package:location/location.dart';
import 'package:shared_preferences/shared_preferences.dart';


class LocationController extends GetxController {
  static late SharedPreferences pref;
  RxString city = 'Choose your current location'.obs;
  RxBool isLocating = false.obs;
  static LocationController instance = Get.find();
  RxDouble latitude = (0.0).obs;
  RxDouble longitude = (0.0).obs;

  @override
  void onInit() async {
    city = (await loadLocation()).obs;
    print(city);
    super.onInit();
  }

  static storeLocation(String city) async {
    pref = await SharedPreferences.getInstance();
    pref.setString("location", city);
  }

  static storePosition(String longitude, String latitude) async {
    pref = await SharedPreferences.getInstance();
    pref.setString("longitude", longitude);
    pref.setString("latitude", latitude);
  }

  static Future<String> loadLocation() async {
    pref = await SharedPreferences.getInstance();
    String city = pref.getString("location")!;
    LocationController.instance.setCity(city);
    return city;
  }

  Future<void> getMyLocation() async {
    Location location = Location();
    bool _serviceEnabled;
    PermissionStatus _permissionGranted;
    LocationData _locationData;
    setIsLocating(false);
    //check if the location service enabled or not
    _serviceEnabled = await location.serviceEnabled();
    if (!_serviceEnabled) {
      _serviceEnabled = await location.requestService();
      if (!_serviceEnabled) {
        return;
      }
    }

    //check the location permission is granted or not
    _permissionGranted = await location.hasPermission();
    if (_permissionGranted == PermissionStatus.denied) {
      _permissionGranted = await location.requestPermission();
      if (_permissionGranted != PermissionStatus.granted) {
        return;
      }
    }

    setIsLocating(true);
    //getting the current location
    _locationData = await location.getLocation();
    // print(_locationData);
    storePosition(_locationData.longitude!.toString(), _locationData.latitude!.toString());
    var address = await geo.GeocodingPlatform.instance.placemarkFromCoordinates(
      _locationData.latitude!,
      _locationData.longitude!,
    );
    isLocating(false);
    setCity(address[0].thoroughfare! + ", " + address[0].subAdministrativeArea! + ", " + address[0].administrativeArea!);
  }

  setCity(String myCity) async {
    city = myCity.obs;
    await storeLocation(myCity);
    update();
  }

  setIsLocating(bool val) {
    isLocating = val.obs;
    update();
  }
}