import 'package:get/get.dart';

class MainController extends GetxController {
  static MainController instance = Get.find();
  int index = 0;

  @override
  void onInit() {
    super.onInit();
  }

  void changeIndex(int i) {
    index = i;
    update();
  }
}