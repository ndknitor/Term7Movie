// File generated by FlutterFire CLI.
// ignore_for_file: lines_longer_than_80_chars, avoid_classes_with_only_static_members
import 'package:firebase_core/firebase_core.dart' show FirebaseOptions;
import 'package:flutter/foundation.dart'
    show defaultTargetPlatform, kIsWeb, TargetPlatform;

/// Default [FirebaseOptions] for use with your Firebase apps.
///
/// Example:
/// ```dart
/// import 'firebase_options.dart';
/// // ...
/// await Firebase.initializeApp(
///   options: DefaultFirebaseOptions.currentPlatform,
/// );
/// ```
class DefaultFirebaseOptions {
  static FirebaseOptions get currentPlatform {
    if (kIsWeb) {
      return web;
    }
    switch (defaultTargetPlatform) {
      case TargetPlatform.android:
        return android;
      case TargetPlatform.iOS:
        return ios;
      case TargetPlatform.macOS:
        throw UnsupportedError(
          'DefaultFirebaseOptions have not been configured for macos - '
          'you can reconfigure this by running the FlutterFire CLI again.',
        );
      case TargetPlatform.windows:
        throw UnsupportedError(
          'DefaultFirebaseOptions have not been configured for windows - '
          'you can reconfigure this by running the FlutterFire CLI again.',
        );
      case TargetPlatform.linux:
        throw UnsupportedError(
          'DefaultFirebaseOptions have not been configured for linux - '
          'you can reconfigure this by running the FlutterFire CLI again.',
        );
      default:
        throw UnsupportedError(
          'DefaultFirebaseOptions are not supported for this platform.',
        );
    }
  }

  static const FirebaseOptions web = FirebaseOptions(
    apiKey: 'AIzaSyB6MCMoH8JlQBITWu2BwSMUvvOt78QS8sc',
    appId: '1:247506663986:web:d98972f6e06114e1155a2b',
    messagingSenderId: '247506663986',
    projectId: 'fcine-fe5ae',
    authDomain: 'fcine-fe5ae.firebaseapp.com',
    storageBucket: 'fcine-fe5ae.appspot.com',
    measurementId: 'G-J47LMJP6PK',
  );

  static const FirebaseOptions android = FirebaseOptions(
    apiKey: 'AIzaSyD3ZfHixm2gzSlPXGej9qRzqbLsw8pxuP4',
    appId: '1:247506663986:android:2ee65abc1a5a95b7155a2b',
    messagingSenderId: '247506663986',
    projectId: 'fcine-fe5ae',
    storageBucket: 'fcine-fe5ae.appspot.com',
  );

  static const FirebaseOptions ios = FirebaseOptions(
    apiKey: 'AIzaSyDjt9CQPSmluoc3AlnFnfepOUUvEbNiNIk',
    appId: '1:247506663986:ios:09fe70b0e146c8e1155a2b',
    messagingSenderId: '247506663986',
    projectId: 'fcine-fe5ae',
    storageBucket: 'fcine-fe5ae.appspot.com',
    androidClientId: '247506663986-d0aacbo61lu6krms597b8vauvtd66pct.apps.googleusercontent.com',
    iosClientId: '247506663986-rfm2a89ulr6q1tnceuokct3fr0dmms1o.apps.googleusercontent.com',
    iosBundleId: 'com.example.term7moviemobile',
  );
}
