export interface UserI {
  created: string;
  email: string;
  firstName: string;
  id: number;
  isVerified: boolean;
  jwtToken: string;
  lastName: string;
  phoneNumber: string;
  role: string;
}

export interface PersonI {
  identity: number;
  firstName: string;
  surname: string;
  age: number;
  sex: string;
  mobile: string;
  active: string;
}
