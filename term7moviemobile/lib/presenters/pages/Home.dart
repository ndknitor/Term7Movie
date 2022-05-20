import 'package:barcode_widget/barcode_widget.dart';
import 'package:flutter/material.dart';
import 'package:flutter_barcode_scanner/flutter_barcode_scanner.dart';
import 'package:term7moviemobile/presenters/shared/NavigateMenu.dart';
import 'package:term7moviemobile/views/responses/StandardResponse.dart';
import 'package:http/http.dart';

import '../../views/GlobalState.dart';
import '../../libraries/HttpFetch.dart';

class Home extends StatefulWidget {
  @override
  HomeState createState() => HomeState();
}

class HomeState extends State<Home> {
  StandardResponse response = StandardResponse();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Home")),
      drawer: Menu(),
      body: Center(
          child: Column(children: <Widget>[
        Text(response.message == null ? '' : response.message),
        Listener(
          onPointerUp: (e) {
            buttonPess = false;
          },
          child: ElevatedButton(
            onPressed: counterIncrement,
            onLongPress: () {
              buttonPess = true;
              holdCounterIncrement();
            },
            child: Text("Increment"),
          ),
        ),
        Listener(
          child: ElevatedButton(
              onPressed: counterDecrement,
              onLongPress: () {
                buttonPess = true;
                holdCounterDecrement();
              },
              child: Text("Decreasement")),
          onPointerUp: (e) {
            buttonPess = false;
          },
        ),
        ElevatedButton(onPressed: scanQRCode, child: Text("Scan QR code")),
        Text(result),
        TextField(
            controller: textToQrController,
            decoration: InputDecoration(hintText: "Text to QR code")),
        ElevatedButton(onPressed: createQRCode, child: Text("Create QR Code")),
        Container(
          child: BarcodeWidget(
              data: qrGen,
              barcode: Barcode.qrCode(),
              width: 200,
              height: 200,
              color: Colors.black),
        )
      ])),
    );
  }

  bool buttonPess = false;
  String result = "", qrGen = "";
  final textToQrController = TextEditingController();
  Future<void> scanQRCode() async {
    try {
      result = await FlutterBarcodeScanner.scanBarcode(
          "#ff6666", "Cancel", true, ScanMode.QR);
      setState(() {
        this.result = result;
      });
    } catch (e) {}
  }

  void createQRCode() {
    setState(() {
      qrGen = textToQrController.text;
      FocusManager.instance.primaryFocus.unfocus();
    });
  }

  void counterIncrement() async {
    Response res = await HttpFetch.get('/api/home/hello');
    if (res.statusCode == 200) {
      StandardResponse standardResponse = StandardResponse.fromRawJson(res.body);
      setState(() {
        response = standardResponse;
      });
    }
  }

  void counterDecrement() async {
    Response res = await HttpFetch.post('/api/unauthorized/signin', '{"email": "user@example.com","password": "string"}');
    if (res.statusCode == 200) {
      StandardResponse standardResponse = StandardResponse.fromRawJson(res.body);
      setState(() {
        response = standardResponse;
      });
    }
  }

  void holdCounterIncrement() async {
    while (buttonPess) {
      setState(() {
        GlobalState.counter++;
      });
      await Future.delayed(Duration(milliseconds: 50));
    }
  }

  void holdCounterDecrement() async {
    while (buttonPess) {
      setState(() {
        GlobalState.counter--;
      });
      await Future.delayed(Duration(milliseconds: 50));
    }
  }
}
