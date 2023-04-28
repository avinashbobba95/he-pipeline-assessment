import { createStore } from "@stencil/store";

const { state, onChange } = createStore({
  dataDictionary: [],
});

onChange('dataDictionary', value => {
  state.dataDictionary = value;
});

export default state;
