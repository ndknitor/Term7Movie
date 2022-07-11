import 'package:get/get.dart';

class MainController extends GetxController {
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